using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebJaguarPortal.ViewModels
{
    public class UserGridViewModel
    {
        public long Id { get; set; }
        public string Username { get; set; }
        
        [DisplayName("First name")]
        public string FirstName { get; set; }
        
        [DisplayName("Last name")]
        public string LastName { get; set; }
        
        [DisplayName("E-mail")]
        public string Email { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }

    public class UserAddViewModel
    {
        public UserAddViewModel()
        {
            Roles = new();
        }
        
        [DisplayName("Username")]
        [Required]
        public string Username { get; set; }
        
        [DisplayName("First name")]
        public string FirstName { get; set; }
        
        [DisplayName("Last name")]
        public string LastName { get; set; }
        
        [DisplayName("E-mail")]
        
        public string Email { get; set; }
        
        public UserRolesViewModel Roles { get; set; }
    }

    public class UserEditViewModel
    {
        public long Id { get; set; }
        
        [DisplayName("Username")]
        public string Username { get; set; }
        
        [DisplayName("First name")]
        public string FirstName { get; set; }
        
        [DisplayName("Last name")]
        public string LastName { get; set; }
        
        [DisplayName("E-mail")]
        public string Email { get; set; }
        
        public UserRolesViewModel Roles { get; set; }
    }

    public class UserDeleteViewModel
    {
        public long Id { get; set; }
        
        [DisplayName("Username")]
        public string? Username { get; set; }
        
        [DisplayName("First name")]
        public string? FirstName { get; set; }
        
        [DisplayName("Last name")]
        public string? LastName { get; set; }
        
        [DisplayName("E-mail")]
        public string? Email { get; set; }
        
        public UserRolesViewModel Roles { get; set; }
    }
    public class UserDetailViewModel : UserEditViewModel
    {
    }


    public class UserRolesViewModel
    {
        public UserRolesViewModel()
        {
            UsersPermission = new();
            AnalyzesPermission = new();
            ProjectsPermission = new();
        }
        
        [DisplayName("Is administrator?")]
        public bool IsAdmin { get; set; }

        public UserPermissionsViewModel UsersPermission { get; set; }

        public UserPermissionsViewModel AnalyzesPermission { get; set; }

        public UserPermissionsViewModel ProjectsPermission { get; set; }
    }

    public class UserPermissionsViewModel
    {
        public bool List { get; set; }
        public bool Detail { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool New { get; set; }
    }
}
