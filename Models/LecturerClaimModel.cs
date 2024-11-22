namespace CMCS.Models // namespace for CMCS models
{
    public class LecturerClaimModel // model for representing a lecturer's claim details
    {
        public int HoursWorked { get; set; } // number of hours worked by the lecturer
        public decimal HourlyRate { get; set; } // hourly rate for the lecturer's work

        // property to calculate total payment based on hours worked and hourly rate
        public decimal TotalPayment => HoursWorked * HourlyRate; // calculates total payment
    }
}