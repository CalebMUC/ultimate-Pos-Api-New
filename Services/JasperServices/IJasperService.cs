using Ultimate_POS_Api.DTOS.Reports;
using static Ultimate_POS_Api.Models.Reports;

namespace Ultimate_POS_Api.Services.JasperServices
{
    public interface IJasperService
    {
        public Task<byte[]> GetJasperReportsAsync(JasperExportData jasperExportData);
        public Task<byte[]> ExpoertStockReportAsync(ExportStockReportDto dto);
    }
}
