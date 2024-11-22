
namespace CMCS.Models
{
    // Enumeration to define the various statuses a claim can have
    public enum ClaimStatus
    {
        PendingVerification, // claim waiting verification
        Approved,            // claim approved
        Rejected             // claim rejected
    }

    public class VerifyClaimsModel
    {
        public List<ClaimForVerification> Claims { get; set; } = new List<ClaimForVerification>(); // list of claims for verification
        public int ClaimId { get; set; } // unique identifier for the claim
        public string LecturerName { get; set; } // name of the lecturer associated with the claim
        public decimal HoursWorked { get; set; } // total hours worked related to the claim
        public decimal HourlyRate { get; set; } // hourly rate for the work performed
        public string Notes { get; set; } // additional notes regarding the claim
        public string DocumentFileName { get; set; } // associated document name
        public ClaimStatus Status { get; set; } // current status of the claim
    }
}
