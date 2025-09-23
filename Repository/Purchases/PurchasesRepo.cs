using Microsoft.EntityFrameworkCore;
using Ultimate_POS_Api.Data;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.DTOS.Purchases;
using Ultimate_POS_Api.Models;

namespace Ultimate_POS_Api.Repository.Purchases
{
    public class PurchasesRepo : IPurchaseRepo
    {
        private readonly UltimateDBContext _dbContext; 
        private ILogger<PurchasesRepo> _logger;

        public PurchasesRepo(UltimateDBContext dBContext, ILogger<PurchasesRepo> logger)
        {
            _dbContext = dBContext;
            _logger = logger;
        }

        public async Task<ResponseStatus> AddPurchaseOrderAsync(AddPurchaseOrderDto dto)
        {
            if (dto == null)
            {
                return new ResponseStatus { Status = 400, StatusMessage = "Invalid request: Purchase order data is missing." };
            }

            if (dto.Items == null || !dto.Items.Any())
            {
                return new ResponseStatus { Status = 400, StatusMessage = "Purchase order must contain at least one item." };
            }

            try
            {
                // 🔹 Validate Supplier
                var supplierExists = await _dbContext.Supplier.AnyAsync(s => s.SupplierId == dto.SupplierId);
                if (!supplierExists)
                {
                    return new ResponseStatus { Status = 404, StatusMessage = "Invalid Supplier. Supplier not found." };
                }

                // 🔹 Validate Products
                var productIds = dto.Items.Select(i => i.ProductId).Distinct().ToList();
                var validProductIds = await _dbContext.Products
                    .Where(p => productIds.Contains(p.ProductID))
                    .Select(p => p.ProductID)
                    .ToListAsync();

                var invalidProducts = productIds.Except(validProductIds).ToList();
                if (invalidProducts.Any())
                {
                    return new ResponseStatus
                    {
                        Status = 404,
                        StatusMessage = $"Invalid Product(s): {string.Join(", ", invalidProducts)}"
                    };
                }

                using var transaction = await _dbContext.Database.BeginTransactionAsync();

                // 🔹 Create new Purchase Order
                var newOrder = new PurchaseOrders
                {
                    PurchaseOrderId = Guid.NewGuid(),
                    OrderNumber = string.IsNullOrWhiteSpace(dto.OrderNumber)
                        ? $"PO-{DateTime.UtcNow:yyyyMMddHHmmss}" // auto-generate if missing
                        : dto.OrderNumber,
                    SupplierId = dto.SupplierId,
                    OrderDate = dto.OrderDate,
                    DeliveryDate = dto.DeliveryDate,
                    TotalAmount = dto.TotalAmount > 0 ? dto.TotalAmount : dto.Items.Sum(i => i.Quantity * i.UnitPrice),
                    Status = string.IsNullOrWhiteSpace(dto.Status) ? "Pending" : dto.Status,
                    IsActive = true,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = dto.CreatedBy,
                    Notes = dto.Notes
                };

                // 🔹 Attach Items
                foreach (var item in dto.Items)
                {
                    newOrder.Items.Add(new PurchaseOrderItems
                    {
                        PurchaseOrderItemId = Guid.NewGuid(),
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.TotalPrice > 0 ? item.TotalPrice : item.Quantity * item.UnitPrice,
                        IsActive = true,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = dto.CreatedBy
                    });
                }

                await _dbContext.PurchaseOrders.AddAsync(newOrder);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ResponseStatus { Status = 201, StatusMessage = "Purchase order created successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating purchase order");
                return new ResponseStatus { Status = 500, StatusMessage = "An error occurred while creating the purchase order." };
            }
        }

        public async Task<List<GetPurchaseOrder>> GetAllPurchaseOrdersAsync()
        {
            var orders = await _dbContext.PurchaseOrders
                .Include(po => po.Supplier)   // 🔹 Supplier info
                .Include(po => po.Items)
                    .ThenInclude(i => i.Product) // 🔹 Product info
                .Where(po => po.IsActive)
                .ToListAsync();

            return orders.Select(order => new GetPurchaseOrder
            {
                PurchaseOrderId = order.PurchaseOrderId,
                OrderNumber = order.OrderNumber,
                SupplierId = order.SupplierId,
                SupplierName = order.Supplier?.SupplierName,
                //SupplierContact = order.Supplier?.c,
                SupplierEmail = order.Supplier?.Email,
                OrderDate = order.OrderDate,
                DeliveryDate = order.DeliveryDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                IsActive = order.IsActive,
                CreatedBy = order.CreatedBy,
                CreatedOn = order.CreatedOn,
                UpdatedBy = order.UpdatedBy,
                UpdatedOn = order.UpdatedOn,
                Notes = order.Notes,

                Items = order.Items.Select(i => new GetPurchaseOrderItem
                {
                    PurchaseOrderItemId = i.PurchaseOrderItemId,
                    PurchaseOrderId = i.PurchaseOrderId,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice,
                    IsActive = i.IsActive,
                    CreatedBy = i.CreatedBy,
                    CreatedOn = i.CreatedOn,
                    UpdatedBy = i.UpdatedBy,
                    UpdatedOn = i.UpdatedOn
                }).ToList()
            }).ToList();
        }

        public async Task<ResponseStatus> DeletePurchaseOrderAsync(Guid purchaseOrderId)
        {
            try
            {
                var order = await _dbContext.PurchaseOrders
                    .Include(po => po.Items)
                    .FirstOrDefaultAsync(po => po.PurchaseOrderId == purchaseOrderId);

                if (order == null)
                {
                    return new ResponseStatus
                    {
                        Status = 404,
                        StatusMessage = "Purchase order not found."
                    };
                }

                // 🔹 Soft delete order
                order.IsActive = false;
                order.UpdatedOn = DateTime.UtcNow;
                order.UpdatedBy = "system"; // TODO: inject current user instead

                // 🔹 Soft delete all items
                foreach (var item in order.Items)
                {
                    item.IsActive = false;
                    item.UpdatedOn = DateTime.UtcNow;
                    item.UpdatedBy = "system"; // TODO: inject current user instead
                }

                await _dbContext.SaveChangesAsync();

                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = "Purchase order deleted successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting purchase order {PurchaseOrderId}", purchaseOrderId);
                return new ResponseStatus
                {
                    Status = 500,
                    StatusMessage = "An error occurred while deleting the purchase order."
                };
            }
        }



    }


}
