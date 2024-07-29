using Microsoft.Build.Evaluation;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebJaguarPortal.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        

        [ForeignKey("Roles")]
        public long? RolesId { get; set; }
        public virtual UserRoles? Roles { get; set; }
    }

    public class UserRoles
    {
        public long Id { get; set; }
        public bool IsAdmin { get; set; }

        [ForeignKey("UserPermissions")]
        public long? UsersPermissionId { get; set; }
        public virtual UserPermissions? UsersPermission { get; set; }
        
        [ForeignKey("AnalyzesPermission")]
        public long? AnalyzesPermissionId { get; set; }
        public virtual UserPermissions? AnalyzesPermission { get; set; }
        
        [ForeignKey("ProjectsPermission")]
        public long? ProjectsPermissionId { get; set; }
        public virtual UserPermissions? ProjectsPermission { get; set; }
    }

    public class UserPermissions
    {
        public long Id { get; set; }
        public bool List { get; set; }
        public bool Detail { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool New { get; set; }
    }
}
