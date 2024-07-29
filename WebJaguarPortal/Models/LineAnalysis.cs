using System.ComponentModel.DataAnnotations.Schema;

namespace WebJaguarPortal.Models
{
    public class LineAnalysis
    {
        public long Id { get; set; }
        public long ClassAnalysisId { get; set; }

        [ForeignKey(nameof(ClassAnalysisId))]
        public virtual ClassAnalysis Class { get; set; }
        public string Method { get; set; }

        public int NumberLine { get; set; }

        public int Cef { get; set; }

        public int Cep { get; set; }

        public int Cnf { get; set; }

        public int Cnp { get; set; }

        public double SuspiciousValue { get; set; }
    }
}
