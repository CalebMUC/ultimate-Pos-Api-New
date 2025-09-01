using Quartz;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Data;
using System;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ultimate_POS_Api.Repository;
using Ultimate_POS_Api.Models;
using System.Text.Json;

namespace Ultimate_POS_Api.Helper
{
    public class AccountSettlementJob : IJob
    {
        private readonly ILogger<AccountSettlementJob> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly FluxApiSettings _fluxApiSettings;

        private readonly UltimateDBContext _dbContext;

        public AccountSettlementJob(
           ILogger<AccountSettlementJob> logger,
           IHttpClientFactory httpClientFactory,
           IAccountRepository accountRepository,
           UltimateDBContext dbContext,
           IOptions<FluxApiSettings> fluxApiOptions)
        {
            _logger = logger;
            _dbContext = dbContext;
            _accountRepository = accountRepository;
            _fluxApiSettings = fluxApiOptions.Value;
        }


        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("AccountSettlementJob started at {DateTime}", DateTime.Now);

            var dbtransaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var snapshotDateTime = DateTime.UtcNow;

                //fetch all accounts
                var AccountTrx = await _accountRepository.GetAccounts();

                var settlementRecords = new List<AccountTrxSettlement>();

                foreach (var accountTransaction in AccountTrx)
                {
                    settlementRecords.Add(new AccountTrxSettlement
                    {
                        DateTime = accountTransaction.CreatedOn,
                        AccountId = accountTransaction.AccountId,
                        UserId = accountTransaction.UserID,
                        SettledOpeningBalance = accountTransaction.OpeningBalance,
                        SettledClosingBalance = accountTransaction.OpeningBalance + accountTransaction.ClearBalance, // Or ClearBalance if you kept it
                        SettledAccountStatus = accountTransaction.AccountStatus,
                        ProcessedAt = snapshotDateTime
                    });
                }
                // Add to settlement table
                await _dbContext.AccountTrxSettlement.AddRangeAsync(settlementRecords);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("{Count} settlement records added at {DateTime}", settlementRecords.Count, DateTime.Now);

                // 3. Clear/Reset balances in Account table
                foreach (var accountTransaction in AccountTrx)
                {
                    accountTransaction.ClearBalance = 0;
                    accountTransaction.OpeningBalance = 0;
                    //accountTransaction.ClosingBalance = 0;
                    accountTransaction.AccountStatus = false;
                }

                await _dbContext.SaveChangesAsync();

                await dbtransaction.CommitAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during AccountSettlementJob execution.");
            }

            _logger.LogInformation("AccountSettlementJob finished at {DateTime}", DateTime.Now);
        }

    }
}