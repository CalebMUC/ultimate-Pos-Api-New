using System.Data;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.DTOS.Reports;
using Ultimate_POS_Api.Models;

namespace Ultimate_POS_Api.Repository
{
    public interface IReportRepository
    {

        //public Task<DataSet> GetReportData(string Reporttype, Dictionary<string, string> parameters);

        public Task<DataSet> GetReportData(ReportRequestData reportRequestData);
        //public Task<b> GetReportData(ReportRequestData reportRequestData);

        public Task<DataSet> GetLowStockReportAsync(LowStockReportDto lowStockReportDto);

    }
}