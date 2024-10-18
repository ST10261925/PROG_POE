namespace CMCS.Models
{
    public class ClaimForVerification
    {
        public int Id { get; set; }

        
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public DateTime SubmissionDate { get; set; } = DateTime.Now; 
        public string SubmittedBy { get; set; } = "Anonymous";
        public string LecturerName { get; set; } = string.Empty;
        public int HoursWorked { get; set; }
        public decimal HourlyRate { get; set; }
        // nullable if not all claims have documents submitted
        public string? DocumentFileName { get; set; }
        public string Notes { get; set; } = string.Empty; 
        public int ClaimId { get; set; }
    }
}
