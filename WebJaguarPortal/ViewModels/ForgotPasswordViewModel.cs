using System.ComponentModel;

namespace WebJaguarPortal.ViewModels
{
    public class ForgotPasswordViewModel
    {        
        [DisplayName("Username or Email")]
        public string UsernameOrEmail { get; set; }
    }
}
