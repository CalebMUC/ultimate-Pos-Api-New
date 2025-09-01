using Microsoft.EntityFrameworkCore;
using Ultimate_POS_Api.Data;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;
using System.Security.Claims;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http.HttpResults;


namespace Ultimate_POS_Api.Repository
{

    public class SuppliersRepository : ISuppliersRepository
    {

        private readonly UltimateDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<SuppliersRepository> _logger;


        public SuppliersRepository(UltimateDBContext dbContext, IHttpContextAccessor httpContextAccessor, ILogger<SuppliersRepository> logger)
        {

            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;

        }


        public async Task<ResponseStatus> AddSuplier(SuppliersDetailsDTO supplier)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            using var SuppliersScope = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                foreach (SuppliersDTO dto in supplier.Supplier)
                {
                    Supplier newSupplier = new()
                    {
                        // SupplierId = dto.SupplierId,
                        SupplierName = dto.SupplierName,
                        SupplierType = dto.SupplierType,
                        Industry = dto.Industry,
                        KRAPIN = dto.KRAPIN,
                        BusinessLicenseNumber = dto.BusinessLicenseNumber,
                        SupplierStatus = dto.SupplierStatus,
                        Remarks = dto.Remarks,
                        Email = dto.Email,
                        Phone = dto.Phone,
                        LocationName = dto.LocationName,
                        Town = dto.Town,
                        Postal = dto.Postal,
                        ContractStartDate = dto.ContractStartDate,
                        ContractEndDate = dto.ContractEndDate,
                        ContractTerms = dto.ContractTerms,
                        Status = dto.ContractStatus,

                        Category = dto.Category,
                        UnitMeasure = dto.UnitMeasure,
                        BankName = dto.BankName,
                        Bank_AccountNumber = dto.Bank_AccountNumber,
                        Till = dto.Till,
                        Pochi = dto.Pochi,
                        Paybill_BusinessNumber = dto.Paybill_BusinessNumber,
                        Paybill_Account = dto.Paybill_Account,

                        CreatedBy = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                        UpdatedBy = "",

                        CreatedOn = DateTime.UtcNow

                    };
                    // #pragma warning restore CS8601 // Possible null reference assignment.



                    // ContactDetailsJson = JsonConvert.SerializeObject(dto.Contactdetails.Select(S => new
                    // {
                    //     SupplierName = S.Name,
                    //     SupplierEmail = S.Email,
                    //     SuplierPhone = S.Phone
                    // }).ToList()),

                    // AddressDetailsJson = JsonConvert.SerializeObject(dto.AddressDetails.Select(A => new
                    // {
                    //     AddressLocationName = A.LocationName,
                    //     AddressTown = A.Town,
                    //     AddressPostal = A.Postal,

                    // }).ToList()),


                    // ProductInfosJson = JsonConvert.SerializeObject(dto.Productsdetails.Select(P => new
                    // {

                    //     ProductNamee = P.ProductName,
                    //     ProductCategory = P.Category,
                    //     ProductUnitMeasure = P.UnitMeasure,
                    //     // ProductCount = P.Count,

                    // }).ToList()),


                    // ContractDetailsJson = JsonConvert.SerializeObject(dto.ContractDetails.Select(C => new
                    // {

                    //     ContStartDate = C.ContractStartDate,
                    //     ContEndDate = C.ContractEndDate,
                    //     ContTerms = C.Terms,

                    // }).ToList()),  

                    // BankDetailsJson = JsonConvert.SerializeObject(dto.BankAccountDetails.Select(B => new 
                    // {
                    //     B.BankName,
                    //     B.AccountNumber

                    // }).ToList() ),

                    // MpesaDetailsJson = JsonConvert.SerializeObject(dto.MpesaDetails.Select(M => new 
                    // {
                    //     M.Till,
                    //     M.Pochi

                    // }).ToList()),




                    // };



                    // Check if Supplier already exists
                    foreach (var sup in supplier.Supplier)
                    {
                        var existingSupplier = _dbContext.Supplier.FirstOrDefault(x => x.SupplierName != null && x.SupplierName == sup.SupplierName);
                        if (existingSupplier != null)
                        {
                            return new ResponseStatus
                            {
                                Status = 409,
                                StatusMessage = $"Supplier '{sup.SupplierName}' already exist"
                            };
                        }
                        else
                        {

                            // Add the supplier to the context
                            _dbContext.Supplier.Add(newSupplier);
                        }

                    }




                }

                // Save changes to the database
                await _dbContext.SaveChangesAsync();
                await SuppliersScope.CommitAsync();

                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = "Supplier added successfully"
                };
            }
            catch (Exception ex)
            {
                await SuppliersScope.RollbackAsync();

                return new ResponseStatus
                {
                    Status = 500,
                    StatusMessage = $"Internal Server Error: {ex.Message}"
                };
            }
        }

        public async Task<ResponseStatus> EditSuplierAsync(SuppliersDetailsDTO suppliers) {
            try
            {
                foreach(var supplier in suppliers.Supplier)
                {
                    //Get the Existing supplier Details
                    var existingSupplier = await _dbContext.Supplier.FindAsync(supplier.SupplierId);
                    if (existingSupplier != null) {
                        UpdateSupplierFromDto(existingSupplier, supplier);

                        await _dbContext.SaveChangesAsync();
                    }

                    
                }


                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = "Supplier Edited Successfully"
                };


            }
            catch (Exception ex) {

                _logger.LogError($"Error : {ex.Message} ");
                return new ResponseStatus
                {
                    Status = 200,
                    StatusMessage = "Internal Server Error"
                };
            }
        }

        public void UpdateSupplierFromDto( Supplier existingSupplier, SuppliersDTO dto) {
            var user = _httpContextAccessor.HttpContext?.User;
            // SupplierId = dto.SupplierId,
            existingSupplier.SupplierName = dto.SupplierName;
            existingSupplier.SupplierType = dto.SupplierType;
            existingSupplier.Industry = dto.Industry;
            existingSupplier.KRAPIN = dto.KRAPIN;
            existingSupplier.BusinessLicenseNumber = dto.BusinessLicenseNumber;
            existingSupplier.SupplierStatus = dto.SupplierStatus;
            existingSupplier.Remarks = dto.Remarks;
            existingSupplier.Email = dto.Email;
            existingSupplier.Phone = dto.Phone;
            existingSupplier.LocationName = dto.LocationName;
            existingSupplier.Town = dto.Town;
            existingSupplier.Postal = dto.Postal;
            existingSupplier.ContractStartDate = dto.ContractStartDate;
            existingSupplier.ContractEndDate = dto.ContractEndDate;
            existingSupplier.ContractTerms = dto.ContractTerms;
            existingSupplier.Status = dto.ContractStatus;
            //existingSupplier.Category = dto.Category;
            existingSupplier.UnitMeasure = dto.UnitMeasure;
            existingSupplier.BankName = dto.BankName;
            existingSupplier.Bank_AccountNumber = dto.Bank_AccountNumber;
            existingSupplier.Till = dto.Till;
            //existingSupplier.Pochi = dto.Pochi;
            existingSupplier.Paybill_BusinessNumber = dto.Paybill_BusinessNumber;
            existingSupplier.Paybill_Account = dto.Paybill_Account;

            existingSupplier.UpdatedBy = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            existingSupplier.UpdatedOn = DateTime.UtcNow;
        }
        public async Task<IEnumerable<Supplier>> GetSupplier()
        {
            var response = await _dbContext.Supplier.ToListAsync();
            return response;
        }

    }


}


