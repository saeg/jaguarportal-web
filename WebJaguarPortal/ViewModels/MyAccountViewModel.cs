using System.ComponentModel;

namespace WebJaguarPortal.ViewModels
{
    public class MyAccountViewModel
    {
        public string Username { get; set; }
        
        [DisplayName("First name")]
        public string FirstName { get; set; }
        
        [DisplayName("Last name")]
        public string LastName { get; set; }
        
        [DisplayName("Current Password")]
        public string CurrentPassword { get; set; }
        
        public string Password { get; set; }
        
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
        
        [DisplayName("Client Id")]
        public string ClientId { get; set; }
        
        [DisplayName("Client Secret")]
        public string ClientSecret { get; set; }
        
        [DisplayName("E-mail")]
        public string Email { get; set; }
    }

    public class MyAccountPersonalDataViewModel
    {
        [DisplayName("First name")]
        public string FirstName { get; set; }
        
        [DisplayName("Last name")]
        public string LastName { get; set; }
        
        [DisplayName("E-mail")]
        public string Email { get; set; }
    }

    public class MyAccountPasswordViewModel
    {
        public string Password { get; set; }
     
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
        
        [DisplayName("Current Password")]
        public string CurrentPassword { get; set; }
    }    
}
