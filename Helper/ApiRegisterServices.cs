using Ultimate_POS_Api.Repository;
using Ultimate_POS_Api.DTOS;
using Quartz;
using Ultimate_POS_Api.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Ultimate_POS_Api.Repository.Authentication;
using Ultimate_POS_Api.Repository.TillRepo;
using Ultimate_POS_Api.Repository.Finance;
using Ultimate_POS_Api.Repository.Purchases;
using Ultimate_POS_Api.Repository.Invoices;

namespace Ultimate_POS_Api.Helper
{
    public static class ApiRegisterServices
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {

            // Register Services     
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ItransactionRepository, TransactionRepository>();
            services.AddScoped<ISuppliersRepository, SuppliersRepository>();
            services.AddScoped<ISuppliesRepository, SuppliesRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IproductsRepository, ProductsRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<ICatalogueRepository, CatalogueRepository>();
            services.AddScoped<IMpesaRepository, MpesaRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<IDocumentRepository, DocumentsRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICommonRepository, CommonRepository>();
            services.AddScoped<ITillRepository, TillRepository>();
            services.AddScoped<IFinanceRepo, FinanceRepo>();
            services.AddScoped<IPurchaseRepo,PurchasesRepo>();
            services.AddScoped<iInvoiceRepo, InvoiceRepo>();



            services.AddScoped<DocumentService>();

            services.Configure<MpesaSettings>(configuration.GetSection("Mpesa"));
            services.Configure<FluxApiSettings>(configuration.GetSection("FluxApi"));
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddTransient<NotificationJob>();


            services.AddHttpClient("FluxApi", client =>
           {
               client.BaseAddress = new Uri(configuration["FluxApi:ApiUrl"]!);
               client.DefaultRequestHeaders.Add("Accept", "application/json");
               client.Timeout = TimeSpan.FromSeconds(30);
           });

            // --- Quartz.NET Configuration ---
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
                q.UseInMemoryStore();

                // ---- NotificationJob ----
                if (configuration.GetValue<bool>("QuartzJobs:EnableNotificationJob"))
                {
                    var SMSSenderJobKey = new JobKey("NotificationServiceJob", "ScheduledOperations");
                    q.AddJob<NotificationJob>(opts => opts.WithIdentity(SMSSenderJobKey));

                    q.AddTrigger(opts => opts
                        .ForJob(SMSSenderJobKey)
                        .WithIdentity("NotificationServiceJob-Trigger")
                        .WithSimpleSchedule(x => x
                            .WithIntervalInMinutes(2) // every 2 minutes
                            .RepeatForever())
                    );
                }

                // ---- AccountSettlementJob ----
                if (configuration.GetValue<bool>("QuartzJobs:EnableAccountSettlementJob"))
                {
                    var accountSettlementJobKey = new JobKey("AccountSettlementJob", "DailyOperations");
                    q.AddJob<AccountSettlementJob>(opts => opts.WithIdentity(accountSettlementJobKey));

                    q.AddTrigger(opts => opts
                        .ForJob(accountSettlementJobKey)
                        .WithIdentity("AccountSettlementJob-DailyTrigger")
                        .WithCronSchedule("0 5 0 * * ?"));// 00:05 AM daily
                }
            });


            // Quartz.NET as a hosted service
            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            return services;
        }
    }

}


