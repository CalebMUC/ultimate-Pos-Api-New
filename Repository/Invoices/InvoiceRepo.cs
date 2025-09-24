using Microsoft.EntityFrameworkCore;
using Ultimate_POS_Api.Data;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.DTOS.Invoices;
using Ultimate_POS_Api.Models;
using Ultimate_POS_Api.Repository.Purchases;

namespace Ultimate_POS_Api.Repository.Invoices
{
    public class InvoiceRepo : iInvoiceRepo
    {
        private readonly UltimateDBContext _dbContext;
        private ILogger<InvoiceRepo> _logger;

        public InvoiceRepo(UltimateDBContext dBContext, ILogger<InvoiceRepo> logger)
        {
            _dbContext = dBContext;
            _logger = logger;
        }

        public async Task<List<GetPurchasesInvoice>> GetPurchaseInvoicesAsync()
        {
            try
            {
                return await _dbContext.PurchaseInvoices
                    .Select(pi => new GetPurchasesInvoice
                    {
                        PurchaseInvoiceId = pi.PurchaseInvoiceId,
                        PurchaseOrderId = pi.PurchaseOrderId,
                        InvoiceNumber = pi.InvoiceNumber,
                        InvoiceDate = pi.InvoiceDate,
                        DueDate = pi.DueDate,
                        TotalAmount = pi.TotalAmount,
                        TaxAmount = pi.TaxAmount,
                        Status = pi.Status,
                        Notes = pi.Notes,
                        IsActive = pi.IsActive,
                        CreatedBy = pi.CreatedBy,
                        CreatedOn = pi.CreatedOn,
                        UpdatedBy = pi.UpdatedBy,
                        UpdatedOn = pi.UpdatedOn
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching purchase invoices");
                return new List<GetPurchasesInvoice>();
            }
        }


        public async Task<ResponseStatus> AddPurchaseInvoiceAsync(PurchasesInvoiceDto dto)
        {
            try
            {
                // 🔹 Check PO exists
                var po = await _dbContext.PurchaseOrders
                    .Include(p => p.Items)
                    .FirstOrDefaultAsync(p => p.PurchaseOrderId == dto.PurchaseOrderId && p.IsActive);

                if (po == null)
                {
                    return new ResponseStatus { Status = 404, StatusMessage = "Purchase order not found." };
                }

                // 🔹 Validate totals match PO
                var expectedTotal = po.Items.Sum(i => i.TotalPrice);
                if (dto.TotalAmount != expectedTotal)
                {
                    return new ResponseStatus
                    {
                        Status = 400,
                        StatusMessage = $"Invoice total {dto.TotalAmount} does not match Purchase Order total {expectedTotal}"
                    };
                }

                var invoice = new PurchaseInvoices
                {
                    PurchaseInvoiceId = Guid.NewGuid(),
                    PurchaseOrderId = dto.PurchaseOrderId,
                    InvoiceNumber = dto.InvoiceNumber,
                    InvoiceDate = dto.InvoiceDate,
                    DueDate = dto.DueDate,
                    TotalAmount = dto.TotalAmount,
                    TaxAmount = dto.TaxAmount,
                    Status = "Pending Approval",
                    Notes = dto.Notes,
                    IsActive = true,
                    CreatedBy = dto.CreatedBy,
                    CreatedOn = DateTime.UtcNow
                };

                _dbContext.PurchaseInvoices.Add(invoice);
                await _dbContext.SaveChangesAsync();

                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = "Purchase invoice created successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating purchase invoice");
                return new ResponseStatus { Status = 500, StatusMessage = "An error occurred while creating invoice." };
            }
        }

        public async Task<ResponseStatus> EditPurchaseInvoiceAsync(EditPurchaseInvoiceDto dto)
        {
            try
            {
                var invoice = await _dbContext.PurchaseInvoices
                    .FirstOrDefaultAsync(i => i.PurchaseInvoiceId == dto.PurchaseInvoiceId && i.IsActive);

                if (invoice == null)
                {
                    return new ResponseStatus { Status = 404, StatusMessage = "Invoice not found." };
                }

                invoice.InvoiceNumber = dto.InvoiceNumber ?? invoice.InvoiceNumber;
                invoice.InvoiceDate = dto.InvoiceDate ?? invoice.InvoiceDate;
                invoice.DueDate = dto.DueDate ?? invoice.DueDate;
                invoice.TotalAmount = dto.TotalAmount ?? invoice.TotalAmount;
                invoice.TaxAmount = dto.TaxAmount ?? invoice.TaxAmount;
                invoice.Status = dto.Status ?? invoice.Status;
                invoice.Notes = dto.Notes ?? invoice.Notes;
                invoice.UpdatedBy = dto.UpdatedBy ?? invoice.UpdatedBy;
                invoice.UpdatedOn = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();

                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = "Purchase invoice updated successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating purchase invoice");
                return new ResponseStatus { Status = 500, StatusMessage = "An error occurred while updating invoice." };
            }
        }

        public async Task<ResponseStatus> DeletePurchaseInvoiceAsync(Guid purchaseInvoiceId)
        {
            try
            {
                var invoice = await _dbContext.PurchaseInvoices
                    .FirstOrDefaultAsync(i => i.PurchaseInvoiceId == purchaseInvoiceId && i.IsActive);

                if (invoice == null)
                {
                    return new ResponseStatus { Status = 404, StatusMessage = "Invoice not found." };
                }

                invoice.IsActive = false;
                invoice.UpdatedOn = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();

                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = "Purchase invoice deleted successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting purchase invoice");
                return new ResponseStatus { Status = 500, StatusMessage = "An error occurred while deleting invoice." };
            }
        }





    }
}
