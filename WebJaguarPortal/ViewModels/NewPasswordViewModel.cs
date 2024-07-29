using System.ComponentModel;

namespace WebJaguarPortal.ViewModels
{
    public class NewPasswordViewModel
    {
        public string Password { get; set; }
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
        public string? Message { get; set; }
        public string? Key { get; set; }
    }
}
