using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Text.Json.Serialization;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;
using Ultimate_POS_Api.ReportMapping;
using Ultimate_POS_Api.Repository;

namespace Ultimate_POS_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UltimateController : ControllerBase
    {
        private readonly IReportRepository _service;
        public UltimateController(IReportRepository services)
        {
            _service = services;
        }

        //[HttpPost("Generate")]
        //public async Task<IActionResult> GenerateReport([FromBody] ReportRequestData requestData)
        //{
        //    try
        //    {
        //        if (!ValidateParameters(requestData.ReportType, requestData.parameters))
        //        {
        //            return BadRequest("inValid Parameters");
        //        }

        //       // var reportData = await _service.GetReportData(requestData.ReportType, requestData.parameters);

        //        var response = ConvertDataTableToList(reportData.Tables[0]);

        //        return Ok(response);

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //}

        //private bool ValidateParameters(string reportType, Dictionary<string, string> parameters)
        //{

        //    if (ReportConfiguration.ReportParameterMappings.TryGetValue(reportType, out var requiredParameters))

        //        //  if (!ReportMapping.ReportConfiguration.ReportParameterMappings.TryGetValue(reportType, out var requiredParameters))
        //        return false;

        //    foreach (var requiredParameter in requiredParameters)
        //    {
        //        if (!parameters.ContainsKey(requiredParameter) || string.IsNullOrWhiteSpace(parameters[requiredParameter]))
        //        {
        //            return false;
        //        }
        //    }
        //    return true;

        //}

        private List<Dictionary<string, object>> ConvertDataTableToList(DataTable table)
        {
            var result = new List<Dictionary<string, object>>();

            foreach (DataRow row in table.Rows)
            {
                var rowDict = new Dictionary<string, object>();
                foreach (DataColumn column in table.Columns)
                {
                    rowDict[column.ColumnName] = row[column];
                }
                result.Add(rowDict);
            }

            return result;
        }


    }
}
