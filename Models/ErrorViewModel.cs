
namespace CMCS.Models // namespace for CMCS models
{
    public class ErrorViewModel // model for representing error information
    {
        public string? RequestId { get; set; } // optional request identifier for tracking errors

        // property to determine if the RequestId should be shown
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId); // returns true if RequestId is not null or empty
    }
}