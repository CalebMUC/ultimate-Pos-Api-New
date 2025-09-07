using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Ultimate_POS_Api.Data;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.DTOS.Permissions;
using Ultimate_POS_Api.DTOS.Roles;
using Ultimate_POS_Api.DTOS.Users;
using Ultimate_POS_Api.Helper;
using Ultimate_POS_Api.Models;


namespace Ultimate_POS_Api.Repository.Authentication;

public class AuthenticationRepository : IAuthenticationRepository
{

    private readonly UltimateDBContext _dbContext;
    private readonly JwtSettings _jwtSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _config;

    private readonly IAccountRepository _accountRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthenticationRepository> _logger;

    public AuthenticationRepository(UltimateDBContext dbContext, 
        IOptions<JwtSettings> jwtsettings, 
        IConfiguration config,
        IHttpContextAccessor httpContextAccessor,
        IAccountRepository accountRepository,
        ILogger<AuthenticationRepository> logger)
    {
        _dbContext = dbContext;
        _jwtSettings = jwtsettings.Value;
        _httpContextAccessor = httpContextAccessor;
        _config = config;
        _accountRepository = accountRepository;
        _logger = logger;
    }

    //public async Task<ResponseStatus> Register(Userdto register)
    //{
    //    byte[] salt;
    //    // var user = _httpContextAccessor.HttpContext?.User;
    //    try
    //    {
    //        //check if user already exists
    //        var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.PhoneNumber == register.phoneNumber ||
    //        u.Email == register.Email);
    //        if (existingUser != null)
    //        {
    //            return new ResponseStatus
    //            {
    //                Status = 400,
    //                StatusMessage = "User Already Exists",
    //            };
    //        }
    //        //int roleId = _dbContext.Role
    //        //             .Where(r => r.RoleName == register.RoleName)
    //        //             .Select(r => r.RoleId)
    //        //             .FirstOrDefault();
    //        //if (roleId == 0)
    //        //{
    //        //    return new ResponseStatus
    //        //    {
    //        //        Status = 400,
    //        //        StatusMessage = "Role not found",
    //        //    };
    //        //}

    //        var hashedPassword = HashPassword(register.Password, out salt);
    //        // Create the user
    //        var newUser = new User
    //        {
    //            UserId = Guid.NewGuid(),
    //            UserName = register.UserName,
    //            Email = register.Email,
    //            PhoneNumber = register.phoneNumber,
    //            Password = hashedPassword,
    //            Salt = Convert.ToBase64String(salt),
    //            RoleId = register.RoleID,
    //            RoleName = register.RoleName,
    //            CreatedOn = DateTime.UtcNow,
    //            UpdatedOn = DateTime.UtcNow,
    //            CreatedBy = register.CreatedBy,//user?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
    //            UpdatedBy = "",

    //        };

    //        _dbContext.Users.Add(newUser);

    //        await _dbContext.SaveChangesAsync();

    //        var role = await _dbContext.Role.FirstOrDefaultAsync(a => a.RoleName == register.RoleName);

    //        //if (role.AccessLevel == 5)
    //        //{
    //        //    var CreatAccount = new AccountDto
    //        //    {
    //        //        UserID = newUser.UserId
    //        //    };

    //        //    var res = await _accountRepository.AddAccount(CreatAccount);

    //        //}
    //        return new ResponseStatus
    //        {
    //            Status = 200,
    //            StatusMessage = "User Created SuccessFully"
    //        };

    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(ex.Message);
    //    }

    //}

    public async Task<ResponseStatus> Register(Userdto register)
    {
        try
        {
            // ✅ Check if user already exists (by email or phone)
            var existingUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == register.PhoneNumber || u.Email == register.Email);

            if (existingUser != null)
            {
                return new ResponseStatus
                {
                    Status = 400,
                    StatusMessage = "User already exists"
                };
            }

            // ✅ Validate Role
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleId == register.RoleId);
            if (role == null)
            {
                return new ResponseStatus
                {
                    Status = 400,
                    StatusMessage = "Invalid role"
                };
            }

            // ✅ Hash password
            var hashedPassword = HashPassword(register.Password, out byte[] salt);

            // ✅ Create user
            var newUser = new User
            {
                UserId = Guid.NewGuid(),
                UserName = register.UserName,
                Email = register.Email,
                PhoneNumber = register.PhoneNumber,
                Password = hashedPassword,
                Salt = Convert.ToBase64String(salt),
                RoleId = register.RoleId,   // only FK needed
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                CreatedBy = register.CreatedBy,
                UpdatedBy = ""
            };

            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            return new ResponseStatus
            {
                Status = 200,
                StatusMessage = "User created successfully"
            };
        }
        catch (Exception ex)
        {
            // Optional: log error properly instead of exposing details
            return new ResponseStatus
            {
                Status = 500,
                StatusMessage = $"Internal Server Error: {ex.Message}"
            };
        }
    }




    public async Task<LoginResponseStatus> Login(UserInfo userInfo)
    {
        try
        {
            // 1. Find user by email
            var user = await _dbContext.Users
                                       .FirstOrDefaultAsync(u => u.Email == userInfo.Email);

            if (user == null)
            {
                return new LoginResponseStatus
                {
                    Status = 400,
                    StatusMessage = "User doesn't exist"
                };
            }

            // 2. Verify password
            string storedHash = user.Password;
            byte[] storedSalt = Convert.FromBase64String(user.Salt);

            bool isPasswordValid = VerifyPassword(userInfo.Password, storedHash, storedSalt);

            if (!isPasswordValid)
            {
                return new LoginResponseStatus
                {
                    Status = 401,
                    StatusMessage = "Invalid password"
                };
            }

            // 3. Generate JWT
            string token = GenerateJwtToken(user);


            // 4. Create new session record
            var session = new UserSession
            {
                UserId = user.UserId,
                LoginTime = DateTime.UtcNow,
                DeviceInfo = userInfo.DeviceInfo ?? "Unknown",
                IpAddress = userInfo.IpAddress ?? "Unknown",
                IsActive = true
            };

            _dbContext.UserSessions.Add(session);
            await _dbContext.SaveChangesAsync();

            // 5. Return response
            return new LoginResponseStatus
            {
                Status = 200,
                StatusMessage = "Login successful",
                Token = token,
                Name = user.UserName,
                SessionId = session.SessionId
            };
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred during login: " + ex.Message);
        }
    }

    public async Task<LoginResponseStatus> Logout(Guid userId, Guid sessionId)
    {
        try
        {
            // 1. Find active session for this user & sessionId
            var session = await _dbContext.UserSessions
                                          .FirstOrDefaultAsync(s => s.UserId == userId
                                                                 && s.SessionId == sessionId
                                                                 && s.LogoutTime == null);

            if (session == null)
            {
                return new LoginResponseStatus
                {
                    Status = 404,
                    StatusMessage = "Active session not found"
                };
            }

            // 2. Mark logout
            session.LogoutTime = DateTime.UtcNow;
            session.IsActive = false;
            _dbContext.UserSessions.Update(session);
            await _dbContext.SaveChangesAsync();

            return new LoginResponseStatus
            {
                Status = 200,
                StatusMessage = "Logout successful"
            };
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred during logout: " + ex.Message);
        }
    }


    public async Task<IEnumerable<GetRolesDto>> GetRolesAsync()
    {
        try
        {
            var response = await _dbContext.Roles.Select(r => new GetRolesDto
            {
                RoleId = r.RoleId,
                RoleName = r.RoleName,
                Description = r.Description,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                CreatedBy = r.CreatedBy,
                UpdatedBy = r.UpdatedBy,
                IsSystemRole = r.IsSystemRole,
                IsActive = r.IsActive,
            }).ToListAsync();
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetRolesAsync");
            throw new Exception(ex.Message);
        }
    }

    public async Task<IEnumerable<GetPermissionDto>> GetPermissionsAsync()
    {
        try
        {
            var response = await _dbContext.Permission.Select(pm => new GetPermissionDto
            {
                PermissionId = pm.PermissionId,
                PermissionName = pm.PermissionName,
                Description = pm.Description,
                Module = pm.Module,
                CreatedAt = pm.CreatedAt,
                UpdatedAt = pm.UpdatedAt,
                IsActive = pm.IsActive,
            }).ToListAsync();

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetPermissionsAsync");
            throw new Exception(ex.Message);
        }

    }

    public async Task<IEnumerable<RolePermissionDto>> GetRolePermissionsAsync(RolePermissionRequest request)
    {
        try {
            var response = await _dbContext.RolePermission.Where(rp => rp.RoleId == request.RoleId)
               .GroupBy(rp => rp.RoleId)
               .Select(g => new RolePermissionDto
               {
                   RoleId = g.Key,
                   Permissions = g.Select(rp => rp.Permission.PermissionName).ToList()
               }).ToListAsync();

            return response;



        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error in GetRolePermissionsAsync");
            throw new Exception(ex.Message);
        }
    }


    public async Task<IEnumerable<PermissionModules>> GetPermissionsModulesAsync()
    {
        try
        {
            var response = await _dbContext.Permission
               .GroupBy(p => p.Module )
               .Select(g => new PermissionModules
               {
                    Module= g.Key,
                   Permissions = g.Select(p => p.PermissionName).ToList()
               }).ToListAsync();

            return response;



        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetPermissionsModulesAsync");
            throw new Exception(ex.Message);
        }
    }

    public async Task<ResponseStatus> AddRolesAsync(AddRoleDto addRole)
    {
        try {
            var newRole = new Roles
            {
                RoleName = addRole.RoleName,
                Description = addRole.Description,
                CreatedBy = addRole.CreatedBy,
                IsActive = addRole.IsActive,
                CreatedAt = DateTime.UtcNow,
                IsSystemRole = false,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = addRole.CreatedBy
            };

            await _dbContext.AddAsync(newRole);
            await _dbContext.SaveChangesAsync();

            return new ResponseStatus
            {
                Status = 200,
                StatusMessage = "Role Added Successfully"
            };

        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error Adding Roles");
            throw new Exception(ex.Message);
        }
    }

    public async Task<ResponseStatus> SaveRolePermissionsAsync(SaveRolePermissionsDto dto)
    {
        try
        {
            // ✅ Load role including existing permissions
            var role = await _dbContext.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.RoleId == dto.RoleId);

            if (role == null)
            {
                return new ResponseStatus
                {
                    Status = 404,
                    StatusMessage = "Role not found"
                };
            }

            // ✅ Remove old permissions
            _dbContext.RolePermission.RemoveRange(role.Permissions);

            // ✅ Add new permissions
            var newRolePermissions = dto.Permissions.Select(permId => new RolePermissions
            {
                RoleId = dto.RoleId,
                PermissionId = permId,
                GrantedBy = "System",
                GrantedAt = DateTime.UtcNow
            });

            await _dbContext.RolePermission.AddRangeAsync(newRolePermissions);

            await _dbContext.SaveChangesAsync();

            return new ResponseStatus
            {
                Status = 200,
                StatusMessage = "Permissions updated successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving role permissions");
            throw;
        }
    }


    public string GenerateJwtToken(User user)
    {
        //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var keyBytes = Convert.FromBase64String(_jwtSettings.Key);
        var key = new SymmetricSecurityKey(keyBytes);

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // var claims = new[]
        // {
        //     new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        //     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //     new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        //     new Claim(ClaimTypes.Role, user.RoleId.ToString())
        // };

        var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName), // Better than JwtRegisteredClaimNames.Sub
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, user.RoleId.ToString())
                };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string HashPassword(string password, out byte[] salt)
    {
        salt = new byte[128 / 8];

        try
        {
            //Generate a random number and generate bytea
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }

    private bool VerifyPassword(string enteredPassword, string storedHash, byte[] storedSalt)
    {
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: enteredPassword,
            salt: storedSalt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return hashed == storedHash;
    }

    //public async Task<ResponseStatus> AddRole(RoleDto userRole)
    //{
    //    try
    //    {
    //        //var user = _httpContextAccessor.HttpContext?.User;
    //        var newRole = new Roles
    //        {
    //            RoleName = userRole.RoleName,
    //            AccessLevel = GetAccessLevel(userRole.RoleName),
    //            CreatedOn = DateTime.UtcNow,
    //            UpdatedOn = DateTime.UtcNow,
    //            CreatedBy = "FELO",//user?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
    //            // CreatedBy = userRole.CreatedBy,
    //            UpdatedBy = ""
    //        };

    //        _dbContext.Roles.Add(newRole);

    //        await _dbContext.SaveChangesAsync();

    //        return new ResponseStatus
    //        {
    //            Status = 200,
    //            StatusMessage = "newRole added SuccessFully"
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(ex.Message);
    //    }
    //}

    public async Task<IEnumerable<Roles>> GetUserRole()
    {
        var response = await _dbContext.Roles.ToListAsync();

        return response;
    }

    public async Task<IEnumerable<GetUsersDto>> GetUsers()
    {
        var response = await _dbContext.Users
            .Include(u => u.Role)
            .Select(user => new GetUsersDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                RoleId = user.RoleId,
                RoleName = user.Role.RoleName, // ✅ direct from navigation
         
                UpdatedBy = user.UpdatedBy,
                UpdatedOn = user.UpdatedOn,
                CreatedBy = user.CreatedBy,
                CreatedOn = user.CreatedOn,
            })
            .ToListAsync();

        return response;
    }

    public async Task<IEnumerable<GetUsersDto>> GetCashiersAsync()
    {
        var response = await _dbContext.Users
            .Include(u => u.Role)
            .Where(u => u.Role.RoleName == "Cashier") // ✅ filter only Cashiers
            .Select(user => new GetUsersDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                RoleId = user.RoleId,
                RoleName = user.Role.RoleName, // ✅ direct from navigation
                UpdatedBy = user.UpdatedBy,
                UpdatedOn = user.UpdatedOn,
                CreatedBy = user.CreatedBy,
                CreatedOn = user.CreatedOn,
            })
            .ToListAsync();

        return response;
    }


    private int GetAccessLevel(string roleName)
    {
        return roleName switch
        {
            "Admin" => 10,
            "Cashier" => 5,
            "Guest" => 1,
            _ => 0 // Default access level for unknown roles
        };
    }


    //public async Task<ResponseStatus> AddPermissions(RolePermissionDto rolePermissionDto)
    //{
    //    var user = _httpContextAccessor.HttpContext?.User;
    //    try
    //    {

    //        foreach (var dto in rolePermissionDto.Submodules)
    //        {
    //            var Submoduleslist = JsonConvert.SerializeObject(dto); //
    //            var Permissions = JsonConvert.SerializeObject(dto.Permissions);


    //            var newPermission = new RolePermissions
    //            {
    //                RoleId = rolePermissionDto.RoleId,
    //                ModuleName = rolePermissionDto.ModuleName,
    //                Submodules = Submoduleslist,
    //                Permissions = Permissions,
    //                CreatedOn = DateTime.UtcNow,
    //                UpdatedOn = DateTime.UtcNow,
    //                CreatedBy = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
    //                UpdatedBy = ""
    //            };

    //            _dbContext.RolePermission.Add(newPermission);
    //        }
    //        await _dbContext.SaveChangesAsync();

    //        return new ResponseStatus
    //        {
    //            Status = 200,
    //            StatusMessage = "newRole added SuccessFully"
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(ex.Message);
    //    }

    //    return new ResponseStatus
    //    {
    //        Status = 200,
    //        StatusMessage = "newRole added SuccessFully"
    //    };
    //}
}
