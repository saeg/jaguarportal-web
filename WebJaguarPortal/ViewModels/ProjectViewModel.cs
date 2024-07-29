using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebJaguarPortal.ViewModels
{
    public class ProjectGridViewModel
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class ProjectAddViewModel
    {
        [MaxLength(50)]        
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
    }

    public class ProjectEditViewModel
    {
        public long Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }

        public string Key { get; set; }
    }

    public class ProjectDeleteViewModel : ProjectEditViewModel
    {
    }
    public class ProjectDetailViewModel : ProjectEditViewModel
    {
    }
}
