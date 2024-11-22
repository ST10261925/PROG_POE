
namespace CMCS.Models // namespace for CMCS models
{
    public class Claim // model for claims
    {
        public int HoursWorked { get; set; } // number of hours worked
        public decimal HourlyRate { get; set; } // hourly rate for the claim
        public string Notes { get; set; } = string.Empty; // additional notes for the claim
        public string Status { get; set; } = "Pending"; // status of the claim, default is pending
    }
}