namespace CMCS.Models // namespace for CMCS models
{
    public class ViewClaimsModel // model for representing the details of a claim to be viewed
    {
        public int ClaimId { get; set; } // unique identifier for the claim

        public string ClaimTitle { get; set; } = string.Empty; // title of the claim, initialized to an empty string
        public string ClaimStatus { get; set; } = "Pending"; // current status of the claim, default is "Pending"
        public decimal ClaimAmount { get; set; } // total amount associated with the claim
        public DateTime SubmissionDate { get; set; } = DateTime.Now; // date when the claim was submitted, default is current date and time
    }
}