
namespace CMCS.Models // namespace for CMCS models
{
    public class ApprovedClaimsModel // model for approved claims
    {
        public int ClaimId { get; set; } // unique identifier for claim

        public string ClaimTitle { get; set; } = string.Empty; // title of the claim

        public decimal ClaimAmount { get; set; } // amount of the claim

        public DateTime ApprovalDate { get; set; } // date when claim was approved

        public string LecturerName { get; set; } = string.Empty; // name of the lecturer
    }
}