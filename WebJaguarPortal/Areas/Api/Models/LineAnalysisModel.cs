namespace WebJaguarPortal.Areas.Api.Models
{
    public class LineAnalysisModel
    {
        public string Method { get; set; }

        public int NumberLine { get; set; }

        public int Cef { get; set; }

        public int Cep { get; set; }

        public int Cnf { get; set; }

        public int Cnp { get; set; }

        public double SuspiciousValue { get; set; }
    }
}
