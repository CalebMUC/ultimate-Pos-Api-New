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
using Ultimate_POS_Api.Controllers;

namespace Ultimate_POS_Api.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly UltimateDBContext _dbContext;
        private readonly IConfiguration _configuration;

        public DashboardRepository(UltimateDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task GetAllAverages()
        {
            throw new NotImplementedException();
        }

        public Task GetGraphData()
        {
            throw new NotImplementedException();
        }
    }
}