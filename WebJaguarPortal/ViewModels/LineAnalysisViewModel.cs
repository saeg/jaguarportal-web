using System.ComponentModel.DataAnnotations.Schema;

namespace WebJaguarPortal.ViewModels
{
    public class LineAnalysisViewModel
    {
        public long Id { get; set; }

        public string Method { get; set; }
        public string ClassName { get; set; }
        public string ClassId { get; set; }

        public int NumberLine { get; set; }

        public int Cef { get; set; }

        public int Cep { get; set; }

        public int Cnf { get; set; }

        public int Cnp { get; set; }

        public double SuspiciousValue { get; set; }
    }
}
