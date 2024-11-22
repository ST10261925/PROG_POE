using CMCS.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class ClaimsController : Controller
{
    private readonly IWebHostEnvironment _webHostEnvironment; // enviroment for hosting
    private readonly ILogger<ClaimsController> _logger; // logger for logging info
    private const int MaxFileSize = 10 * 1024 * 1024; // this here is 10 MB
    private static readonly string[] AllowedFileTypes = { ".pdf", ".docx", ".xlsx" }; // allowed file types

    public ClaimsController(IWebHostEnvironment webHostEnvironment, ILogger<ClaimsController> logger)
    {
        _webHostEnvironment = webHostEnvironment; // setting web host env
        _logger = logger; 
    }

    [HttpGet]
    public IActionResult SubClaim() 
    {
        return View("SubClaim"); // returns subclaim view
    }

    [HttpPost]
    public async Task<IActionResult> SubClaim(SubClaimModel model, IFormFile document) // post method for subclaim
    {
        try
        {
            if (!ModelState.IsValid) 
            {
                return View("SubClaim", model); // return view with model if invalid
            }

            string uniqueFileName = null; 

            if (document != null) // check if document is provided
            {
                if (document.Length > MaxFileSize) // check file size
                {
                    ModelState.AddModelError("Document", "File size must be less than 10MB"); // error for size
                    return View("SubClaim", model); // return view with model
                }

                var fileExtension = Path.GetExtension(document.FileName).ToLowerInvariant(); // get file extension
                if (!AllowedFileTypes.Contains(fileExtension)) // check allowed types
                {
                    ModelState.AddModelError("Document", "Only PDF, DOCX, and XLSX files are allowed"); // error for type
                    return View("SubClaim", model); // return view with model
                }

                uniqueFileName = Guid.NewGuid().ToString() + fileExtension; // generate unique filename
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads"); // set uploads folder
                Directory.CreateDirectory(uploadsFolder); // create directory if not exists
                var filePath = Path.Combine(uploadsFolder, uniqueFileName); // set file path

                using (var stream = new FileStream(filePath, FileMode.Create)) // create file stream
                {
                    await document.CopyToAsync(stream); // copy document to stream
                }
            }

            var claim = new ClaimForVerification // create new claim
            {
                LecturerName = User.Identity?.Name ?? "Anonymous", // get lecturer name
                HoursWorked = model.HoursWorked, // set hours worked
                HourlyRate = model.HourlyRate, // set hourly rate
                Notes = model.Notes ?? string.Empty, // set notes
                DocumentFileName = uniqueFileName, // set document filename
                Status = "Pending" // set initial status
            };

            LocalClaimStorage.AddClaim(claim); // add claim to storage
            _logger.LogInformation($"Claims count after submission: {LocalClaimStorage.GetAllClaims().Count}"); // log claims count

            return RedirectToAction("ViewClaims"); // redirect to view claims
        }
        catch (Exception ex) // catch any exceptions
        {
            _logger.LogError($"Error in SubClaim POST: {ex.Message}"); // log error
            ViewBag.Message = "An error occurred while submitting your claim."; // set error message
            return View("SubClaim", model); // return view with model
        }
    }

    public IActionResult ViewClaims() // view claims method
    {
        var claims = LocalClaimStorage.GetAllClaims(); // get all claims
        return View("ViewClaims", claims); // return view with claims
    }

    public IActionResult ApprovedClaims() // approved claims method
    {
        var approvedClaims = LocalClaimStorage.GetApprovedClaims(); // get approved claims
        return View("ApprovedClaims", approvedClaims); // return view with approved claims
    }

    /*public IActionResult VerifyClaims() // verify claims method
    {
        var claimsToVerify = LocalClaimStorage.GetClaimsToVerify(); // get claims to verify
        return View("VerifyClaims", claimsToVerify); // return view with claims to verify
    }*/

    //[HttpPost]
    public IActionResult ApproveClaim(int id) // approve claim method
    {
        var claim = LocalClaimStorage.GetClaimById(id); // get claim by id
        if (claim == null) // check if claim exists
        {
            // Handle the case where the claim is not found
            _logger.LogWarning($"Claim with ID {id} not found."); // log warning if not found
            return RedirectToAction("VerifyClaims"); // redirect to verify claims
        }

        claim.Status = "Approved"; // set claim status to approved
        LocalClaimStorage.UpdateClaim(claim); // update claim in storage
        _logger.LogInformation($"Claim with ID {id} approved."); // log approval

        return RedirectToAction("VerifyClaims"); // redirect to verify claims
    }

    public IActionResult UploadDoc() // upload document method
    {
        return View("UploadDoc"); // return upload document view
    }
}