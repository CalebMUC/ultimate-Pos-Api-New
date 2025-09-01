using System.Data;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.DTOS.Reports;
using Ultimate_POS_Api.ReportMapping;
using Ultimate_POS_Api.Repository;
using Ultimate_POS_Api.Services.JasperServices;
using static Ultimate_POS_Api.Models.Reports;

namespace Ultimate_POS_Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;
        private readonly ILogger<ReportsController> _logger;
        private readonly IJasperService  _jasperService;

        public ReportsController(IReportRepository reportRepository,ILogger<ReportsController> logger,IJasperService jasperService) { 
            _reportRepository = reportRepository;
            _logger = logger;
            _jasperService = jasperService;
        }
        [HttpPost("Generate")]
        public async Task<IActionResult> GenerateReportData([FromBody] ReportRequestData requestData)
        {
            try
            {

                var response = await _reportRepository.GetReportData(requestData);

                var reportData = convertDataTableToList(response.Tables[0]);

                return Ok(reportData);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("GenerateLowStockReport")]
        public async Task<IActionResult> GenerateLowStockReport([FromBody] LowStockReportDto dto)
        {
            try
            {
                if (dto == null) {
                    return BadRequest("Invalid Low Stock Report Data Provided");
                }

                if ( dto.LowStockThreshold <= 0)
                {
                    return BadRequest("Low Stock Threshold must be greater than zero.");
                }

                var response = await _reportRepository.GetLowStockReportAsync(dto);

                if (response == null || response.Tables.Count == 0 || response.Tables[0].Rows.Count == 0)
                {
                    return NotFound("No low stock items found.");
                }
                var reportData = convertDataTableToList(response.Tables[0]);

                return Ok(reportData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating low stock report");
                return BadRequest("An error occurred while generating the low stock report.");
            }
        }

        [HttpPost("ExportStockReports")]
        public async Task<IActionResult> ExportStockReports([FromBody] ExportStockReportDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Invalid Low Stock Report Data Provided");
                }

                if (dto.lowStockThreshold <= 0)
                {
                    return BadRequest("Low Stock Threshold must be greater than zero.");
                }

                var response = await _jasperService.ExpoertStockReportAsync(dto);

                var contentType = "application/pdf";
                var fileName = $"LowStockReport_{DateTime.Now:yyyyMMddHHmmss}.pdf";
              
                return File(response, contentType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating low stock report");
                return BadRequest("An error occurred while generating the low stock report.");
            }
        }
        [HttpPost("JasperReports")]
        public async Task<IActionResult> GenerateJasperReports([FromBody] JasperExportData jasperExportData)
        {
            try
            {
                if (jasperExportData == null || string.IsNullOrEmpty(jasperExportData.Format) || jasperExportData.Parameters == null)
                {
                    return BadRequest("Invalid Jasper Export Data Provided");
                }
                //check if the incoming ReportType exists in the ReportDefinitions
                //IF IT EXISTS IT WILL RETURN THE DEFINITION, OTHERWISE IT WILL RETURN NULL
                var normalizedReportType = (jasperExportData.ReportType ?? "").Replace(" ", "").Trim();
                if (!ReportConfiguration.ReportDefinitions.TryGetValue(normalizedReportType, out var reportDef))
                {
                    return BadRequest($"Report Type '{jasperExportData.ReportType}' does not exist in the report definitions.");
                }
                var response = await _jasperService.GetJasperReportsAsync(jasperExportData);
                if (response == null || response.Length == 0)
                {
                    return BadRequest("The report content is empty.");
                }
                var contentType = "application/pdf";
                var fileName = $"{jasperExportData.ReportType.Replace(" ", "")}.pdf";
                return File(response, contentType, fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }


        //[HttpPost("Export")]
        //public async Task<IActionResult> Export([FromBody] ReportRequestData requestData)
        //{

        //    try
        //    {
        //        var reportServerUrl = "http://desktop-oe12rm4/ReportServer";
        //        var reportPath = $"UltimateReports/{Uri.EscapeDataString(requestData.ReportType.Replace(" ", ""))}";
        //        var format = requestData.Format.ToUpper();

        //        var parameters = string.Join("&", requestData.parameters.Select(p => $"{Uri.EscapeDataString(p.Key)} = {Uri.EscapeDataString(p.Key)} "));

        //        var reportUrl = $"{reportServerUrl}?{reportPath}&rs:Command=Renders&rs:Format={format}&{parameters}";

        //        //configure HttpHandler for windows Authentication
        //        var handler = new HttpClientHandler
        //        {
        //            Credentials = new NetworkCredential("ADMIN", "dennis@2543#", "DESKTOP-OE12RM4"),
        //            UseDefaultCredentials = false
        //        };

        //        using var httpClient = new HttpClient(handler);

        //        var response = await httpClient.GetAsync(reportUrl);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            return BadRequest($"Failed to fetch report: {response.StatusCode} - {response.ReasonPhrase}");
        //        }

        //        //validate and read the ResponseContent
        //        var reportData = await response.Content.ReadAsByteArrayAsync();

        //        if (reportData == null || reportData.Length == 0)
        //        {
        //            return BadRequest("The report content is empty.");
        //        }

        //        var contentTypeFromResponse = response.Content.Headers.ContentType?.MediaType;

        //        if (contentTypeFromResponse != null && contentTypeFromResponse.Contains("text/html"))
        //        {

        //            var errorMessage = System.Text.Encoding.UTF8.GetString(reportData);
        //            return BadRequest($"The server returned an error: {errorMessage}");

        //        }

        //        // Set the content type and file extension based on the requested format
        //        var contentType = format switch
        //        {
        //            "PDF" => "application/pdf",
        //            "EXCELOPENXML" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //            "WORDOPENXML" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        //            _ => throw new ArgumentException($"Unsupported format: {format}")
        //        };


        //        var fileExtension = format switch
        //        {
        //            "PDF" => "pdf",
        //            "EXCELOPENXML" => "xlsx",
        //            "WORDOPENXML" => "docx",
        //            _ => throw new ArgumentException($"Unsupported format: {format}")
        //        };

        //        // Return the report file to the client
        //        var fileName = $"{requestData.ReportType.Replace(" ", "")}.{fileExtension}";
        //        return File(reportData, contentType, fileName);


        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //}

        private List<Dictionary<string, object>> convertDataTableToList(DataTable dataTable)
        {
            var result = new List<Dictionary<string, object>>();

            foreach (DataRow row in dataTable.Rows)
            {
                var rowDict = new Dictionary<string, object>();

                foreach (DataColumn column in dataTable.Columns)
                {
                    rowDict[column.ColumnName] = row[column];
                }

                result.Add(rowDict);
            }

            return result;
        }
    }
}
