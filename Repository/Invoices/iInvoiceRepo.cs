using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.DTOS.Invoices;
using Ultimate_POS_Api.Models;

namespace Ultimate_POS_Api.Repository.Invoices
{
    public interface iInvoiceRepo
    {
        Task<ResponseStatus> AddPurchaseInvoiceAsync(PurchasesInvoiceDto dto);
        Task<ResponseStatus> EditPurchaseInvoiceAsync(EditPurchaseInvoiceDto dto);
        Task<ResponseStatus> DeletePurchaseInvoiceAsync(Guid purchaseInvoiceId);

        Task<List<GetPurchasesInvoice>> GetPurchaseInvoicesAsync();

        //Task<PurchaseInvoices?> GetInvoiceByIdAsync(Guid invoiceId);
        //Task<IEnumerable<PurchaseInvoices>> GetInvoicesByPurchaseOrderAsync(Guid purchaseOrderId);
    }
}
