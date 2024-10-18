namespace CMCS.Models
{
    public class SubClaimModel
    {
        public int ClaimId { get; set; }
        public int HoursWorked { get; set; }
        public decimal HourlyRate { get; set; }

        // Initialize to an empty string to avoid null reference
        public string Notes { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending"; 

        
        public string? LecturerId { get; set; }

        // Initialize to the current date and time
        public DateTime ClaimDate { get; set; } = DateTime.Now;

        // Add the Document property for file uploads
        public IFormFile Document { get; set; } 
    }
}

