using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Ultimate_POS_Api.DTOS
{
    public class AuthDTO
    {

    }

    public class userdetails
    {
        public string strUser { get; set; } = string.Empty;

        public string strBranch { get; set; } = string.Empty;

        public ushort uAccessLevel { get; set; } = 0;

        public bool IsAdmin { get; set; } = false;

    }

    public class Userdto
    {


        //public string Salt { get; set; } = string.Empty;
        //public string ReEnterPassword { get; set; } = string.Empty;
        //public Guid UserId { get; set; } 

        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Guid RoleId { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;

    }



    public class RoleDto
    {
        //public Guid RoleId { get; set; }
        //public int AccessLevel { get; set; }
        public string RoleName { get; set; } = string.Empty;

        // public DateTime CreatedOn { get; set; }
        // public DateTime? UpdatedOn { get; set; }
        // public string CreatedBy { get; set; } = string.Empty;
        // public string UpdatedBy { get; set; } = string.Empty;
    }

    //public class RolePermissionDto
    //{
    //    // public Guid RoleBaseId { get; set; }

    //    public int RoleId { get; set; }

    //    public string ModuleName { get; set; }

    //    // public List<SubmoduleListDto> Submodules { get; set; }
    //    public List<SubmoduleListDto> Submodules { get; set; } = new List<SubmoduleListDto>();

    //    //public DateTime CreatedAt { get; set; }

    //    //public DateTime CreatedOn { get; set; }

    //    //public DateTime UpdatedOn { get; set; }

    //    //public string CreatedBy { get; set; }

    //    //public string UpdatedBy { get; set; }
    //}

    public class SubmoduleListDto
    {
        public string SubmoduleName { get; set; }
        public List<PermissionsDto> Permissions { get; set; } // Permissions stored as JSON
    }

    public class PermissionsDto
    {
        public string PermissionID { get; set; }
        public string Name { get; set; }
        public bool AllowedAccess { get; set; }

    }


}
