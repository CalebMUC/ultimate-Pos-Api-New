using Microsoft.EntityFrameworkCore;
using Ultimate_POS_Api.Data;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Bibliography;

namespace Ultimate_POS_Api.Repository
{

    public class TransactionRepository : ItransactionRepository
    {
        private readonly UltimateDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger _logger;

        public TransactionRepository(UltimateDBContext dbContext,
            IHttpContextAccessor httpContextAccessor, 
            IAccountRepository accountRepository,
            ILogger<TransactionRepository> logger)
        {
            _dbContext = dbContext;
            _accountRepository = accountRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }


        public async Task<IEnumerable<Transactions>> GetTransactions()
        {
            var response = await _dbContext.Transactions.ToListAsync();
            return response;
        }

        public async Task<ResponseStatus> AddSale(TransactionListDto salesDto)
        {
            await using var transactionScope = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // ✅ Get user info
                var user = _httpContextAccessor.HttpContext.User;
                if (user == null)
                    return new ResponseStatus { Status = 401, StatusMessage = "Unauthorized: User not found" };

                var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
                var cashierName = user.FindFirst(ClaimTypes.Name)?.Value;

                // ✅ Ensure cashier has active till
                var till = await _dbContext.tills
                    .FirstOrDefaultAsync(t => t.UserId == userId && t.Status == "Open");

                if (till == null)
                    return new ResponseStatus { Status = 400, StatusMessage = $"Till is not open for {cashierName}" };

                // ✅ Process each transaction
                foreach (var dto in salesDto.transaction)
                {
                    // 🔹 Process Payments
                    foreach (var payment in dto.PaymentDetails)
                    {
                        var paymentMethod = await _dbContext.payments
                            .FirstOrDefaultAsync(pm => pm.PaymentMethodId == payment.PaymentMethodId && pm.IsActive);

                        if (paymentMethod == null)
                        {
                            await transactionScope.RollbackAsync();
                            return new ResponseStatus { Status = 400, StatusMessage = "Invalid payment method" };
                        }

                        bool success = paymentMethod.Name switch
                        {
                            "Cash" => await ProcessCashTransactions(dto, payment, till),
                            "MPESA" => await ProcessMpesaTransactions(dto, payment, till),
                            _ => false
                        };

                        if (!success)
                        {
                            await transactionScope.RollbackAsync();
                            return new ResponseStatus { Status = 400, StatusMessage = $"{paymentMethod.Name} Transaction Failed" };
                        }
                    }

                    // 🔹 Save Transaction first
                    var transaction = new Transactions
                    {
                        InvoiceNumber = await GenerateInvoiceNumber(),
                        TotalAmount = dto.TotalCost,
                        Discount = dto.TotalDiscount,
                        Tax = dto.TotalValueAddedTax,
                        NetAmount = dto.TotalCost - dto.TotalDiscount + dto.TotalValueAddedTax,
                        TransactionDate = DateTime.UtcNow,
                        Cashier = cashierName,
                        TillId = till.TillId,
                        UserID = userId
                    };

                    _dbContext.Transactions.Add(transaction);
                    await _dbContext.SaveChangesAsync(); // Ensure TransactionId is generated

                    // 🔹 Products
                    foreach (var p in dto.transactionproducts)
                    {
                        var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.SKU == p.SKU);

                        if (product == null)
                        {
                            await transactionScope.RollbackAsync();
                            return new ResponseStatus { Status = 400, StatusMessage = $"Product {p.SKU} not found" };
                        }

                        if (product.Quantity < p.Quantity)
                        {
                            await transactionScope.RollbackAsync();
                            return new ResponseStatus { Status = 400, StatusMessage = $"Insufficient stock for {p.SKU}" };
                        }

                        product.Quantity -= p.Quantity;
                        product.UpdatedOn = DateTime.UtcNow;
                        _dbContext.Products.Update(product);

                        _dbContext.TransactionProducts.Add(new TransactionProducts
                        {
                            TransactionID = transaction.TransactionId,
                            ProductID = product.ProductID,
                            Quantity = p.Quantity,
                            UnitPrice = product.SellingPrice,
                            Discount = p.Discount,
                            SubTotal = (product.SellingPrice * p.Quantity) - p.Discount
                        });
                    }

                    // 🔹 Payments (outside product loop to avoid duplication)
                    foreach (var payment in dto.PaymentDetails)
                    {
                        _dbContext.PaymentDetails.Add(new PaymentDetails
                        {
                            TransactionId = transaction.TransactionId,
                            PaymentMethodId = payment.PaymentMethodId,
                            Name = "Cash",
                            Status = PaymentStatus.Completed,
                            PaymentReference = payment.PaymentReference,
                            Amount = payment.Amount
                        });
                    }
                }

                // 🔹 Save All Changes
                await _dbContext.SaveChangesAsync();
                await transactionScope.CommitAsync();

                return new ResponseStatus { Status = 200, StatusMessage = "Sale processed successfully" };
            }
            catch (Exception ex)
            {
                await transactionScope.RollbackAsync();
                // TODO: Log full exception (stack trace) in a logging service
                _logger.LogError("error", ex.Message);
                return new ResponseStatus { Status = 500, StatusMessage = $"Sale failed: {ex.Message}" };
            }
        }

        public async Task<bool> ProcessCashTransactions(TransactionDto transactionDto, PaymentDetailsDto paymentDetailsDto, Till till)
        {
            try
            {
                var change = paymentDetailsDto.Amount - transactionDto.TotalCost;

                // ✅ Update Till Balances
                till.CurrentAmount += paymentDetailsDto.Amount - Math.Max(change, 0);
                till.ExpectedAmount += paymentDetailsDto.Amount - Math.Max(change, 0);

                _dbContext.tills.Update(till);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ProcessMpesaTransactions(TransactionDto transactionDto, PaymentDetailsDto paymentDetailsDto, Till till)
        {
            try
            {
                // TODO: Integrate with MPESA API (STK Push, C2B, etc.)
                return true;
            }
            catch
            {
                return false;
            }
        }



        private async Task<string> GenerateInvoiceNumber()
        {
            // Format: POS-YYYYMMDD-#### (daily sequence)
            string today = DateTime.UtcNow.ToString("yyyyMMdd");
            string prefix = $"POS-{today}-";

            // Get the last invoice for today
            var lastInvoice = await _dbContext.Transactions
                .Where(t => t.InvoiceNumber.StartsWith(prefix))
                .OrderByDescending(t => t.InvoiceNumber)
                .Select(t => t.InvoiceNumber)
                .FirstOrDefaultAsync();

            int sequence = 1;

            if (lastInvoice != null)
            {
                // Extract last sequence
                var lastSequencePart = lastInvoice.Split('-').Last();
                if (int.TryParse(lastSequencePart, out int lastSeq))
                {
                    sequence = lastSeq + 1;
                }
            }

            return $"{prefix}{sequence:D4}"; // e.g. POS-20250831-0001
        }



        //    public async Task<ResponseStatus> AddSale(TransactionListDto JsonData)
        //    {
        //        Double Amount = 0;
        //        string AccountID = "";
        //        using var transactionScope = await _dbContext.Database.BeginTransactionAsync();
        //        var user = _httpContextAccessor.HttpContext?.User;
        //        var username = user?.FindFirst(ClaimTypes.Name)?.Value;
        //        var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //        string roleid = user?.FindFirst(ClaimTypes.Role)?.Value;
        //        string roleIdString = user?.FindFirst(ClaimTypes.Role)?.Value;
        //        Guid useerId = Guid.Parse(userId);


        //        try
        //        {




        //                foreach (var dto in JsonData.transaction)
        //                {
        //                    var transactionProductsJson = JsonConvert.SerializeObject(dto.transactionproducts);

        //                    var newTransaction = new Transactions
        //                    {
        //                        //TransactionID = dto.TransactionID,c
        //                        UserID = useerId, //dto.UserID,
        //                        TotalValueAddedTax = dto.TotalValueAddedTax,
        //                        TotalCost = dto.TotalCost,
        //                        TotalDiscount = dto.TotalDiscount,
        //                        AmountRecieved = dto.AmountRecieved,
        //                        CashChange = dto.CashChange,
        //                        Quantity = dto.Quantity,
        //                        Remarks = dto.Remarks,
        //                        CreatedOn = DateTime.UtcNow,
        //                        CreatedBy = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
        //                        UpdatedBy = "",
        //                        TransactionProducts = transactionProductsJson
        //                    };

        //                    var accountDetails = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.UserID == useerId);

        //                    AccountID = accountDetails.AccountId.ToString();

        //                    if (AccountID == null)
        //                    {
        //                        return new ResponseStatus
        //                        {
        //                            Status = 400,
        //                            StatusMessage = "Cannot proceed without an account"
        //                        };
        //                    }

        //                    _dbContext.Transactions.Add(newTransaction);

        //                    foreach (var product in dto.transactionproducts)
        //                    {
        //                        //confirm if the products are available      
        //                        var allSkus = await _dbContext.Catalogue.Select(p => p.SKU).ToListAsync();
        //                        var existingProduct = await _dbContext.Catalogue.FirstOrDefaultAsync(p => p.SKU == product.SKU);

        //                        if (existingProduct.Availability == false || existingProduct == null)
        //                        {
        //                            return new ResponseStatus
        //                            {
        //                                Status = 400,
        //                                StatusMessage = $"Insufficient stock for product:" //{newTransaction.TransactionProducts.ProductName}"
        //                            };
        //                        }
        //                        else
        //                        {
        //                            // Update the product availability in catalogue                     
        //                            existingProduct.Availability = false;

        //                            _dbContext.Catalogue.Update(existingProduct);
        //                        }
        //                    }

        //                    // Insert new PaymentDetails records for audit purposes, one per payment method used
        //                    foreach (var paymentDetailDto in dto.PaymentDetails)
        //                    {
        //                        Amount += paymentDetailDto.Amount;

        //                        var newPayment = new PaymentDetails
        //                        {
        //                            //               PaymentID = paymentDetailDto.PaymentID,
        //                            PaymentMethod = paymentDetailDto.PaymentMethod,
        //                            PaymentStatus = paymentDetailDto.PaymentStatus,
        //                            PaymentReference = paymentDetailDto.PaymentReference,
        //                            Amount = paymentDetailDto.Amount,
        //                            // TransactionID = dto.TransactionID,
        //                            PaymentDate = DateTime.UtcNow,
        //                        };
        //                        _dbContext.PaymentDetails.Add(newPayment);
        //                    }
        //                }
        //                await _dbContext.SaveChangesAsync();
        //                await transactionScope.CommitAsync();

        //                //update your account after successfull transaction
        //                UpdateAccountDTO transaction = new UpdateAccountDTO
        //                {
        //                    AccountId = Guid.Parse(AccountID),
        //                    Amount = (decimal)Amount
        //                };

        //                var res = await _accountRepository.UpdateAccount(transaction);


        //                return new ResponseStatus
        //                {
        //                    Status = 200,
        //                    StatusMessage = "Transaction completed successfully"
        //                };

        //        }


        //        catch (Exception ex)
        //        {
        //            // Rollback in case of error
        //            await transactionScope.RollbackAsync();
        //            return new ResponseStatus
        //            {
        //                Status = 500,
        //                StatusMessage = $"Transaction failed: {ex.Message}"
        //            };
        //        }


        //        // }catch (Exception ex)
        //        // {
        //        //     await transactionScope.RollbackAsync();

        //        //     return new ResponseStatus
        //        //     {
        //        //         Status = 500,
        //        //         StatusMessage = $"Internal Server Error: {ex.Message}"
        //        //     };
        //        // }
        //    }


        //    //make payment via mpesa by initiating stk push






        //}
    }
}

