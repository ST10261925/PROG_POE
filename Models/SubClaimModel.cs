

namespace CMCS.Models
{
    public class SubClaimModel
    {
        public int ClaimId { get; set; } // unique identifier for the claim
        public int HoursWorked { get; set; } // number of hours worked related to the claim
        public decimal HourlyRate { get; set; } // hourly rate for the work performed
        public string Notes { get; set; } = string.Empty; // notes or comments regarding the claim
        public string Status { get; set; } = "Pending"; // default status is "Pending"
        public string? LecturerId { get; set; } // optional identifier for the associated lecturer
        public DateTime ClaimDate { get; set; } = DateTime.Now; // default is current date and time
        public IFormFile Document { get; set; } // file associated with the claim (e.g., receipts, documents)
    }
}
