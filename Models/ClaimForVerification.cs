
namespace CMCS.Models // namespace for CMCS models
{
    public class ClaimForVerification // model for claims that need verification
    {
        public int Id { get; set; } // unique identifier for the claim

        public string Description { get; set; } = string.Empty; // description of the claim
        public string Status { get; set; } = "Pending"; // status of the claim, default is pending
        public DateTime SubmissionDate { get; set; } = DateTime.Now; // date when claim was submitted
        public string SubmittedBy { get; set; } = "Anonymous"; // name of the person who submitted the claim
        public string LecturerName { get; set; } = string.Empty; // name of the lecturer related to the claim
        public int HoursWorked { get; set; } // number of hours worked for the claim
        public decimal HourlyRate { get; set; } // hourly rate for the claim
        public string? DocumentFileName { get; set; } // file name of the document, can be null
        public string Notes { get; set; } = string.Empty; // additional notes for the claim
        public int ClaimId { get; set; } // identifier for the related claim
    }
}