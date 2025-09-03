using System;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.DTOS.Permissions;
using Ultimate_POS_Api.DTOS.Roles;
using Ultimate_POS_Api.Models;

namespace Ultimate_POS_Api.Repository.Authentication;


public interface IAuthenticationRepository
{
    public Task<ResponseStatus> Register(Userdto register);

    public Task<LoginResponseStatus> Login(UserInfo userInfo);

    public Task<IEnumerable<Roles>> GetUserRole();

    public Task<ResponseStatus> AddRolesAsync(AddRoleDto addRole);

    public Task<IEnumerable<User>> GetUsers();
    public Task<IEnumerable<GetRolesDto>> GetRolesAsync();
    public Task<IEnumerable<GetPermissionDto>> GetPermissionsAsync();
    public Task<IEnumerable<PermissionModules>> GetPermissionsModulesAsync();
    public Task<IEnumerable<RolePermissionDto>> GetRolePermissionsAsync(RolePermissionRequest request);
    public Task<ResponseStatus> SaveRolePermissionsAsync(SaveRolePermissionsDto dto);
    //public Task<ResponseStatus> AddPermissions(RolePermissionDto rolePermissionDto);


}
