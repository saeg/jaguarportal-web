
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebJaguarPortal.ViewModels
{
    public class SettingsViewModel
    {
        public int Id { get; set; }
        [DisplayName("From")]
        public string? SmtpFrom { get; set; }
        [DisplayName("Password")]
        public string? SmtpPassword { get; set; }
        [DisplayName("Username")]
        public string? SmtpUsername { get; set; }
        [DisplayName("Address")]
        public string? SmtpAddress { get; set; }
        [DisplayName("Use SSL")]
        public bool SmtpUseSSL { get; set; }
        [DisplayName("Port")]
        public int? SmtpPort { get; set; }

        [Required]
        [DisplayName("Entropy Level Password")]
        public int? EntropyLevelPassword { get; set; }
        public List<SelectListItem> EntropyLevelPasswordOptions
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Text = "Select option", Value = "" },
                    new SelectListItem { Text = "Week", Value = "3" },
                    new SelectListItem { Text = "Better", Value = "4" },
                    new SelectListItem { Text = "Strong", Value = "5" }
                };
            }
        }
        public DateTime UpdatedAt { get; set; }
    }
}
