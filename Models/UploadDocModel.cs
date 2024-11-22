namespace CMCS.Models // namespace for CMCS models
{
    public class UploadDocModel // model for representing a document upload
    {
        public IFormFile? UploadedFile { get; set; } // property to hold the uploaded file, nullable to allow no file upload
    }
}