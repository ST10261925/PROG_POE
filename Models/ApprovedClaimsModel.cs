namespace CMCS.Models
{
    public class ApprovedClaimsModel
    {
        public int ClaimId { get; set; }

        public string ClaimTitle { get; set; } = string.Empty;

        public decimal ClaimAmount { get; set; }

        public DateTime ApprovalDate { get; set; }
       
        public string LecturerName { get; set; } = string.Empty;
    }
}
