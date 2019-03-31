namespace EXGEPA.Model
{
    public class OutputCertificate : Certificate
    {
        public OutputType OutputType { get; set; }
    }
    public enum OutputType
    {
        Cession = 1,
        Destruction,
        Disparition,
        Vente
    }
}
