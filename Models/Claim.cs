namespace CMCS.Models
{
    public class Claim
    {
        public int HoursWorked { get; set; }
        public decimal HourlyRate { get; set; }
        public string Notes { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending"; 
    }
}
