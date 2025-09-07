using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Ultimate_POS_Api.Data;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.DTOS.Permissions;
using Ultimate_POS_Api.DTOS.Roles;
using Ultimate_POS_Api.Helper;
using Ultimate_POS_Api.Models;
using Ultimate_POS_Api.ReportMapping;
using Ultimate_POS_Api.Repository;
using Ultimate_POS_Api.Repository.Authentication;

namespace Ultimate_POS_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationRepository _service;
        private readonly UltimateDBContext _Context;
        private readonly ILogger<AuthenticationController> _logger;


        public AuthenticationController(IAuthenticationRepository services, 
            UltimateDBContext ultimateDB,
            ILogger<AuthenticationController> logger)
        {
            _service = services;
            _Context = ultimateDB;
            _logger = logger;
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(UserInfo userInfo)
        {

            try
            {
                var response = await _service.Login(userInfo);

                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        [HttpPost("Register")]
        // [Authorize]
        public async Task<ActionResult> Register(Userdto register)
        {
            try
            {
                var response = await _service.Register(register);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //[HttpPost("AddRole")]
        //// [Authorize]
        //public async Task<ActionResult> AddRole(RoleDto userRole)
        //{
        //    try
        //    {
        //        //var response = await _service.AddRole(userRole);


        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPost("GetRoles")]
        //[Authorize]
        public async Task<ActionResult> GetRoles()
        {
            try
            {
                var response = await _service.GetRolesAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetPermissions")]
        //[Authorize]
        public async Task<ActionResult> GetPermissions()
        {
            try
            {
                var response = await _service.GetPermissionsAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetRolePermissions")]
        //[Authorize]
        public async Task<ActionResult> GetRolePermissions(RolePermissionRequest request)
        {
            try
            {
                var response = await _service.GetRolePermissionsAsync(request);

                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetPermissionsModules")]
        //[Authorize]
        public async Task<ActionResult> GetPermissionsModules()
        {
            try
            {
                var response = await _service.GetPermissionsModulesAsync();

                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetUsers")]
        //[Authorize]
        public async Task<IActionResult> GetUsersAsync([FromQuery] string? searchTerm)
        {
            var response = await _service.GetUsers();

            if (searchTerm == null)
            {
                // Return all users if no search term is provided
                return Ok(response);
            }

            // Filter users if a search term is provided

            var filteredUsers = response
                .Where(u => u.UserId.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();


            return Ok(response);
        }
        [HttpPost("GetCashiers")]
        public async Task<IActionResult> GetCashiers()
        {
            try {
                var response = await _service.GetCashiersAsync();
                return Ok(response);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddRole")]
        public async Task<ActionResult<ResponseStatus>> AddRole([FromBody] AddRoleDto addRole)
        {
            try
            {
                var response = await _service.AddRolesAsync(addRole);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseStatus
                {
                    Status = 400,
                    StatusMessage = ex.Message
                });
            }
        }

        [HttpPost("SaveRolePermissions")]
        public async Task<ActionResult<ResponseStatus>> SaveRolePermissions([FromBody] SaveRolePermissionsDto dto)
        {
            try
            {
                var response = await _service.SaveRolePermissionsAsync(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SaveRolePermissions endpoint");
                return StatusCode(500, new ResponseStatus
                {
                    Status = 500,
                    StatusMessage = ex.Message
                });
            }
        }



        [HttpPost("AddPermissions")]
        [Authorize]
        public async Task<ActionResult> AddPermissions(RolePermissionDto rolePermissionDto)
        {
            try
            {
                //var response = await _service.AddPermissions(rolePermissionDto);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
