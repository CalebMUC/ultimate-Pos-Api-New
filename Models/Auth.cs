using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ultimate_POS_Api.Models;

public class Auth
{
    // Placeholder for authentication logic or models
}

public class User
{
    [Key]
    public Guid UserId { get; set; } = Guid.NewGuid();

    [Required, MaxLength(100)]
    public string UserName { get; set; } = string.Empty;

    [Required, MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public string Salt { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedOn { get; set; }

    [Required]
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }

    // ✅ Role management - pick one approach
    public Guid RoleId { get; set; }
    public Roles Role { get; set; }

    public string RoleName { get; set; } = string.Empty;

    // OR for many-to-many
    //public ICollection<UserRoles> UserRoles { get; set; } = new List<UserRoles>();

    // POS Specific
    public ICollection<Till> Tills { get; set; } = new List<Till>();
}

public class UserSession
{
    [Key]
    public Guid SessionId { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public DateTime LoginTime { get; set; } = DateTime.UtcNow;
    public DateTime? LogoutTime { get; set; }
    public DateTime LastActivity { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    public string DeviceInfo { get; set; }
    public string IpAddress { get; set; }

    public User User { get; set; }
}



//public class Role
//{
//    [Key]
//    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//    public int RoleId { get; set; }

//    [Required]
//    public string RoleName { get; set; } = string.Empty;

//    public int AccessLevel { get; set; }

//    [Required]
//    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

//    [Required]
//    public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;

//    [Required]
//    public string CreatedBy { get; set; } = "System";

//    [Required]
//    public string UpdatedBy { get; set; } = "System";

//    // Navigation property
//    //public ICollection<User> Users { get; set; } = new List<User>();
//}

//public class RolePermission
//{
//    [Key]
//    public Guid RoleBaseId { get; set; }

//    [ForeignKey("Role")]
//    public int RoleId { get; set; }
//    public Role Role { get; set; }

//    [Required]
//    public string ModuleName { get; set; }

//    [Required]
//    public string Submodules { get; set; } = string.Empty;

//    [Required]
//    public string Permissions { get; set; } = string.Empty;


//    //   public List<SubmoduleListDto> Submodules { get; set; }

//    [Required]
//    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

//    [Required]
//    public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;

//    [Required]
//    public string CreatedBy { get; set; }

//    [Required]
//    public string UpdatedBy { get; set; }
//}
