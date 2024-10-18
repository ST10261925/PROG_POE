namespace CMCS.Models
{
    public class ViewClaimsModel
    {
        public int ClaimId { get; set; }

        public string ClaimTitle { get; set; } = string.Empty;
        public string ClaimStatus { get; set; } = "Pending"; 
        public decimal ClaimAmount { get; set; }
        public DateTime SubmissionDate { get; set; } = DateTime.Now;
    }
}
