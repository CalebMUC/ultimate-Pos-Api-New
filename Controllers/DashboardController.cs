using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Ultimate_POS_Api.Data;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;
using Ultimate_POS_Api.Repository;

namespace Ultimate_POS_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        // private readonly IDashboardRepository _dashboardRepository; 
        private readonly UltimateDBContext _dbContext;
        public DashboardController(UltimateDBContext ultimateDB) //DashboardRepository dashboardRepository, 
        {
            // _dashboardRepository = dashboardRepository;
            _dbContext = ultimateDB;
        }

        [HttpGet("SalesAverages")]
        [Authorize]
        public async Task<IActionResult> GetAllAverages()
        {

            try
            {
                var noTransactions = await _dbContext.Transactions.CountAsync();
                var availableProducts = await _dbContext.Products.CountAsync(p => p.Quantity > 0);
                var totalsales = await _dbContext.TransactionProducts.SumAsync(i => (int?)i.Quantity) ?? 0;
                var totalCash = await _dbContext.TransactionProducts.SumAsync(i => (decimal?)(i.Quantity * i.SubTotal)) ?? 0;

                var responseObject = new
                {
                    NoTransactions = noTransactions,
                    AvailableProducts = availableProducts,
                    TotalSales = totalsales,
                    TotalCash = totalCash
                };

                return Ok(responseObject);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }



        // Endpoint to fetch sales for the last 10 days
        [HttpGet("Graphdata")]
        [Authorize]
        public async Task<IActionResult> GetGraphData()
        {
            // Fetch all data from the database
            var allSales = await _dbContext.Transactions.ToListAsync();

            // Get today's date and calculate the date 10 days ago
            var startDate = DateTime.UtcNow.Date.AddDays(-10);

            // Filter, group, and calculate in memory
            var salesForLast10Days = allSales
                .Where(Transactions => Transactions.TransactionDate.Date <= startDate) // Filter sales from the last 10 days
                .GroupBy(Transactions => Transactions.TransactionDate.Date)            // Group by date  Transactions
                .Select(group => new
                {
                    Date = group.Key,                           // Group key (date)
                    TotalSales = group.Sum(Transactions => Transactions.TotalAmount) // Total sales for that date
                })
                .OrderBy(result => result.Date)                 // Order by date ascending
                .ToList();

            return Ok(salesForLast10Days);
        }


        // Endpoint to fetch sales for the last 10 days
        [HttpGet("RecentSales")]
        [Authorize]
        public async Task<IActionResult> GetRecentSales()
        {
            // Fetch all data from the database
            var allSales = await _dbContext.Transactions.ToListAsync();

            // var startDate = DateTime.UtcNow.Date.AddDays(-10);
            var transactions = allSales
            .OrderByDescending(t => t.TransactionDate) // Replace `CreatedDate` with the timestamp column
            .Select(t => new
            {
                t.TransactionId,
                t.TotalAmount,
                //t.Quantity,
                t.TransactionDate.TimeOfDay
            })
            .Take(10)
            .ToList();


            return Ok(transactions);
        }





    }


}