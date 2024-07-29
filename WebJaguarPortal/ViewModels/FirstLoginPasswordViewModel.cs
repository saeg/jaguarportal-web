using System.ComponentModel;

namespace WebJaguarPortal.ViewModels
{
    public class FirstLoginPasswordViewModel
    {        
        [DisplayName("Username or Email")]
        public string UsernameOrEmail { get; set; }
    }
}
