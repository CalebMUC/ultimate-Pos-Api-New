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


        //-----------------------------Sales Invoice --------------------------------

        public async Task<ResponseStatus> AddSalesInvoiceAsync(SalesInvoiceDto dto)
        {
            try
            {
                // ✅ Validate DTO
                if (dto == null || !dto.Items.Any())
                {
                    return new ResponseStatus { Status = 400, StatusMessage = "Invoice must contain at least one item." };
                }

                // ✅ Generate Invoice Number (can customize with sequence/format)
                string invoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMddHHmmss}";

                // ✅ Map to Entity
                var invoice = new SalesInvoice
                {
                    SalesInvoiceId = Guid.NewGuid(),
                    InvoiceNumber = invoiceNumber,
                    InvoiceDate = dto.InvoiceDate,
                    DueDate = dto.DueDate,
                    TaxAmount = dto.TaxAmount,
                    Notes = dto.Notes,
                    CreatedBy = dto.CreatedBy,
                    CreatedOn = DateTime.UtcNow,
                    Status = "Pending",
                    IsActive = true
                };

                // ✅ Map Items
                foreach (var itemDto in dto.Items)
                {
                    var item = new SalesInvoiceItem
                    {
                        SalesInvoiceItemId = Guid.NewGuid(),
                        SalesInvoiceId = invoice.SalesInvoiceId,
                        ProductId = itemDto.ProductId,
                        ProductName = itemDto.ProductName,
                        Quantity = itemDto.Quantity,
                        UnitPrice = itemDto.UnitPrice,
                        TotalAmount = itemDto.Quantity * itemDto.UnitPrice
                    };
                    invoice.SalesInvoiceItems.Add(item);
                }

                // ✅ Calculate Total
                invoice.TotalAmount = invoice.SalesInvoiceItems.Sum(i => i.TotalAmount) + dto.TaxAmount;

                // ✅ Save
                _dbContext.SalesInvoices.Add(invoice);
                await _dbContext.SaveChangesAsync();

                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = $"Sales invoice {invoice.InvoiceNumber} created successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating sales invoice");
                return new ResponseStatus { Status = 500, StatusMessage = "An error occurred while creating sales invoice." };
            }
        }

        public async Task<IEnumerable<GetSalesInvoiceDto>> GetSalesInvoicesAsync()
        {
            try
            {
                var invoices = await _dbContext.SalesInvoices
                    .Include(si => si.SalesInvoiceItems)
                    .Where(si => si.IsActive) // Only active invoices
                    .Select(si => new GetSalesInvoiceDto
                    {
                        InvoiceNumber = si.InvoiceNumber,
                        InvoiceDate = si.InvoiceDate,
                        DueDate = si.DueDate,
                        TotalAmount = si.TotalAmount,
                        TaxAmount = si.TaxAmount,
                        Status = si.Status,
                        Notes = si.Notes,
                        IsActive = si.IsActive,
                        CreatedBy = si.CreatedBy,
                        CreatedOn = si.CreatedOn,
                        UpdatedBy = si.UpdatedBy,
                        UpdatedOn = si.UpdatedOn,

                        // ✅ Map items
                        Items = si.SalesInvoiceItems.Select(item => new GetSalesInvoiceItemDto
                        {
                            ProductId = item.ProductId,
                            ProductName = item.ProductName,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            TotalAmount = item.TotalAmount
                        }).ToList()
                    })
                    .ToListAsync();

                return invoices;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching sales invoices");
                return Enumerable.Empty<GetSalesInvoiceDto>();
            }
        }


        public async Task<ResponseStatus> EditSalesInvoiceAsync(EditSalesInvoiceDto dto)
        {
            try
            {
                var invoice = await _dbContext.SalesInvoices
                    .Include(si => si.SalesInvoiceItems)
                    .FirstOrDefaultAsync(si => si.SalesInvoiceId == dto.SalesInvoiceId && si.IsActive);

                if (invoice == null)
                {
                    return new ResponseStatus { Status = 404, StatusMessage = "Sales invoice not found." };
                }

                // 🔹 Update header fields
                invoice.InvoiceDate = dto.InvoiceDate ?? invoice.InvoiceDate;
                invoice.DueDate = dto.DueDate ?? invoice.DueDate;
                invoice.TotalAmount = dto.TotalAmount ?? invoice.TotalAmount;
                invoice.TaxAmount = dto.TaxAmount ?? invoice.TaxAmount;
                invoice.Status = dto.Status ?? invoice.Status;
                invoice.Notes = dto.Notes ?? invoice.Notes;
                invoice.UpdatedOn = DateTime.UtcNow;
                invoice.UpdatedBy = dto.UpdatedBy ?? invoice.UpdatedBy;

                // 🔹 Update items (if provided)
                if (dto.Items != null)
                {
                    foreach (var itemDto in dto.Items)
                    {
                        if (itemDto.SalesInvoiceItemId.HasValue)
                        {
                            // Update existing item
                            var existingItem = invoice.SalesInvoiceItems
                                .FirstOrDefault(i => i.SalesInvoiceItemId == itemDto.SalesInvoiceItemId.Value);

                            if (existingItem != null)
                            {
                                existingItem.ProductId = itemDto.ProductId;
                                existingItem.ProductName = itemDto.ProductName;
                                existingItem.Quantity = itemDto.Quantity;
                                existingItem.UnitPrice = itemDto.UnitPrice;
                                existingItem.TotalAmount = itemDto.TotalAmount;
                            }
                        }
                        else
                        {
                            // Add new item
                            invoice.SalesInvoiceItems.Add(new SalesInvoiceItem
                            {
                                SalesInvoiceItemId = Guid.NewGuid(),
                                SalesInvoiceId = invoice.SalesInvoiceId,
                                ProductId = itemDto.ProductId,
                                ProductName = itemDto.ProductName,
                                Quantity = itemDto.Quantity,
                                UnitPrice = itemDto.UnitPrice,
                                TotalAmount = itemDto.TotalAmount
                            });
                        }
                    }

                    // 🔹 Optionally: remove items not included in dto.Items
                    var dtoItemIds = dto.Items.Where(i => i.SalesInvoiceItemId.HasValue).Select(i => i.SalesInvoiceItemId.Value).ToList();
                    var itemsToRemove = invoice.SalesInvoiceItems.Where(i => !dtoItemIds.Contains(i.SalesInvoiceItemId)).ToList();
                    foreach (var item in itemsToRemove)
                    {
                        _dbContext.SalesInvoiceItems.Remove(item);
                    }
                }

                await _dbContext.SaveChangesAsync();

                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = "Sales invoice updated successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating sales invoice");
                return new ResponseStatus { Status = 500, StatusMessage = "An error occurred while updating sales invoice." };
            }
        }

        public async Task<ResponseStatus> DeleteSalesInvoiceAsync(Guid salesInvoiceId)
        {
            try
            {
                var invoice = await _dbContext.SalesInvoices
                    .Include(i => i.SalesInvoiceItems)
                    .FirstOrDefaultAsync(i => i.SalesInvoiceId == salesInvoiceId && i.IsActive);

                if (invoice == null)
                {
                    return new ResponseStatus
                    {
                        Status = 404,
                        StatusMessage = "Sales invoice not found or already deleted."
                    };
                }

                // 🔹 Soft delete invoice
                invoice.IsActive = false;
                invoice.UpdatedOn = DateTime.UtcNow;
                invoice.UpdatedBy = "system"; // or pass from user context

                // 🔹 Soft delete items as well (optional)
                foreach (var item in invoice.SalesInvoiceItems)
                {
                    item.TotalAmount = 0; // optional reset
                }

                await _dbContext.SaveChangesAsync();

                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = "Sales invoice deleted successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting sales invoice");
                return new ResponseStatus
                {
                    Status = 500,
                    StatusMessage = "An error occurred while deleting the sales invoice."
                };
            }
        }









    }
}
