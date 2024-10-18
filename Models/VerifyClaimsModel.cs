namespace CMCS.Models
{
    public enum ClaimStatus
    {
        PendingVerification,
        Approved,
        Rejected
    }

    public class ClaimForVerification
    {
        public int ClaimId { get; set; }
        public string LecturerName { get; set; }
        public decimal HoursWorked { get; set; }
        public decimal HourlyRate { get; set; }
        public string Notes { get; set; }
        public string DocumentFileName { get; set; }
        public ClaimStatus Status { get; set; }
    }

    public class VerifyClaimsModel
    {
        public List<ClaimForVerification> Claims { get; set; } = new List<ClaimForVerification>();
    }
}