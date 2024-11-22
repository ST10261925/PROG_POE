namespace CMCS.Models // namespace for CMCS models
{
    public class ClaimSummaryModel // model for summarizing claims
    {
        public int ClaimId { get; set; } // unique identifier for the claim
        public string LecturerName { get; set; } = string.Empty; // name of the lecturer associated with the claim
        public decimal TotalHours { get; set; } // total hours worked for the claim
        public decimal TotalAmount { get; set; } // total amount for the claim
        public string Status { get; set; } = "Pending"; // status of the claim, default is pending
    }
}