using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.DTOS.Reports;
using Ultimate_POS_Api.ReportMapping;
using static Ultimate_POS_Api.Models.Reports;

namespace Ultimate_POS_Api.Services.JasperServices
{
    public class JasperService : IJasperService
    {
        private readonly JasperServiceDto _jasperServiceDto;
        private readonly HttpClient _httpClient;
        private readonly ILogger<JasperService> _logger;

        public JasperService(IOptions<JasperServiceDto> jasperServiceDto, HttpClient httpClient, ILogger<JasperService> logger)
        {
            _jasperServiceDto = jasperServiceDto.Value;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<byte[]> ExpoertStockReportAsync(ExportStockReportDto dto)
        {
            var jasperServiceUrl = _jasperServiceDto.JasperServiceUrl;
            var endpoint = "StockReports";
            var fullUrl = $"{jasperServiceUrl.TrimEnd('/')}/{endpoint}";
            var jsonParam = JsonConvert.SerializeObject(dto);
            try
            {

                using (var request = new HttpRequestMessage(HttpMethod.Post, fullUrl))
                {

                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/pdf"));
                    request.Content = new StringContent(jsonParam, Encoding.UTF8, "application/json");
                    // Log the complete request before sending

                    var requestLog = $"Method: {request.Method}\n" +
                                     $"URL: {request.RequestUri}\n" +
                                     $"Headers:\n" +
                                     $"Request Content : {request.Content}";
                    _logger.LogInformation("Sending HTTP Request:\n{RequestLog}", requestLog);

                    var response = await _httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsByteArrayAsync();


                }

            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP Request failed");
                throw new Exception($"HTTP Request failed: {httpEx.Message}", httpEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching Jasper reports.");
                throw new Exception("An error occurred while fetching Jasper reports.", ex);
            }
        }

        public async Task<byte[]> GetJasperReportsAsync(JasperExportData jasperExportData)
        {
            var normalizedReportType = (jasperExportData.ReportType ?? "").Replace(" ", "").Trim();

            if (!ReportConfiguration.ReportDefinitions.TryGetValue(normalizedReportType, out var reportDef))
            {
                throw new Exception($"Report Type '{jasperExportData.ReportType}' does not exist in the report definitions.");
            }

            var jasperServiceUrl = _jasperServiceDto.JasperServiceUrl;
            var endPoint = reportDef.Endpoint;
            var fullUrl = $"{jasperServiceUrl.TrimEnd('/')}/{endPoint}";

            // Flatten parameters properly
            var cleanedParams = jasperExportData.Parameters.ToDictionary(
                kvp => kvp.Key,
                kvp => ConvertJsonElement(kvp.Value)
            );

            var jsonParam = JsonConvert.SerializeObject(cleanedParams);

            _logger.LogInformation("Preparing Jasper Report request to {Url} with payload: {Payload}", fullUrl, jsonParam);

            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, fullUrl))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/pdf"));
                    request.Content = new StringContent(jsonParam, Encoding.UTF8, "application/json");

                    // Log the complete request before sending
                    var requestLog = new StringBuilder();
                    requestLog.AppendLine($"Method: {request.Method}");
                    requestLog.AppendLine($"URL: {request.RequestUri}");

                    // Log headers
                    requestLog.AppendLine("Headers:");
                    foreach (var header in request.Headers)
                    {
                        requestLog.AppendLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                    }

                    // Log content headers if content exists
                    if (request.Content != null)
                    {
                        foreach (var header in request.Content.Headers)
                        {
                            requestLog.AppendLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                        }
                    }

                    // Log body
                    requestLog.AppendLine("Body:");
                    if (request.Content != null)
                    {
                        var body = await request.Content.ReadAsStringAsync();
                        requestLog.AppendLine(body);
                    }

                    _logger.LogInformation("Sending HTTP Request:\n{RequestLog}", requestLog.ToString());

                    var response = await _httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsByteArrayAsync();
                }
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP Request failed");
                throw new Exception($"HTTP Request failed: {httpEx.Message}", httpEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching Jasper reports.");
                throw new Exception("An error occurred while fetching Jasper reports.", ex);
            }
        }

        /// <summary>
        /// Flattens the input value by converting JsonElement types to usable .NET types
        /// </summary>
        private object ConvertJsonElement(object value)
        {
            if (value is JsonElement element)
            {
                switch (element.ValueKind)
                {
                    case JsonValueKind.Number:
                        return element.TryGetInt64(out var l) ? l : element.GetDecimal();
                    case JsonValueKind.String:
                        return element.GetString();
                    case JsonValueKind.True:
                    case JsonValueKind.False:
                        return element.GetBoolean();
                    case JsonValueKind.Null:
                    case JsonValueKind.Undefined:
                        return null;
                    case JsonValueKind.Object:
                        var nestedDict = new Dictionary<string, object>();
                        foreach (var prop in element.EnumerateObject())
                        {
                            nestedDict[prop.Name] = ConvertJsonElement(prop.Value);
                        }
                        return nestedDict;
                    case JsonValueKind.Array:
                        var list = new List<object>();
                        foreach (var item in element.EnumerateArray())
                        {
                            list.Add(ConvertJsonElement(item));
                        }
                        return list;
                }
            }
            return value;
        }
    }
}
