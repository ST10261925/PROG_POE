using CMCS.Models; // using models from CMCS
using Microsoft.AspNetCore.Mvc; // using ASP.NET core MVC
using System.Diagnostics; // using for diagnostics

namespace CMCS.Controllers // namespace for the CMCS controllers
{
    public class HomeController : Controller // HomeController class inherits from Controller
    {
        private readonly ILogger<HomeController> _logger; // logger for logging errors
        private readonly IWebHostEnvironment _webHostEnvironment; // environment for web hosting
        private const int MaxFileSize = 10 * 1024 * 1024; // max file size set to 10MB
        private readonly string[] AllowedFileTypes = { ".pdf", ".docx", ".xlsx" }; // allowed file types

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment) // constructor
        {
            _logger = logger; // initialize logger
            _webHostEnvironment = webHostEnvironment; // initialize web host environment
        }

        public IActionResult Index() // action for index view
        {
            return View(); // return index view
        }

        public IActionResult LecturerClaim() // action for lecturer claim view
        {
            return View(); // return lecturer claim view
        }

        public IActionResult HRDashboard() // action for HR dashboard
        {
            var claims = new List<ClaimSummaryModel> // create list of claim summaries
            {
                new ClaimSummaryModel { ClaimId = 1, LecturerName = "Jamie Jade", TotalHours = 10, TotalAmount = 200, Status = "Pending" }, // claim 1
                new ClaimSummaryModel { ClaimId = 2, LecturerName = "Jayden Alexander", TotalHours = 15, TotalAmount = 300, Status = "Approved" } // claim 2
            };

            return View(claims); // return HR dashboard view with claims
        }

        public IActionResult ViewClaims() // action for viewing claims
        {
            try
            {
                var claims = LocalClaimStorage.GetAllClaims(); // get all claims from local storage
                return View(claims); // return claims view
            }
            catch (Exception ex) // catch any exceptions
            {
                _logger.LogError($"Error in ViewClaims: {ex.Message}"); // log error
                return RedirectToAction("Error"); // redirect to error page
            }
        }
        // Action for viewing approved claims
        public IActionResult ApprovedClaims()
        {
            try
            {
                var claimsForVerification = LocalClaimStorage.GetApprovedClaims();
                var approvedClaims = claimsForVerification.Select(c => new ApprovedClaimsModel
                {
                    ClaimId = c.Id,
                    ClaimTitle = c.Description,
                    ClaimAmount = c.HoursWorked * c.HourlyRate,
                    ApprovalDate = DateTime.Now,
                    LecturerName = c.LecturerName
                }).ToList();

                return View(approvedClaims);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ApprovedClaims: {ex.Message}");
                return RedirectToAction("Error");
            }
        }
    
[HttpGet] // HTTP GET method
        public IActionResult SubClaim() // action for submitting a claim
        {
            return View(new SubClaimModel()); // return view with new SubClaimModel
        }

        [HttpPost] // HTTP POST method
        public async Task<IActionResult> SubClaim(SubClaimModel model, IFormFile document) // action to submit a claim
        {
            try
            {

                if (!ModelState.IsValid) // check if model state is valid
                {
                    return View(model); // return view with model if invalid
                }

                string uniqueFileName = null; // variable for unique file name

                // Handle file upload
                if (document != null) // check if document is not null
                {
                    // Check file size
                    if (document.Length > MaxFileSize) // check if file size exceeds max limit
                    {
                        ModelState.AddModelError("Document", "File size must be less than 10MB"); // add error to model state
                        return View(model); // return view with model
                    }

                    // Check file type
                    var fileExtension = Path.GetExtension(document.FileName).ToLowerInvariant(); // get file extension
                    if (!AllowedFileTypes.Contains(fileExtension)) // check if file type is allowed
                    {
                        ModelState.AddModelError("Document", "Only PDF, DOCX, and XLSX files are allowed"); // add error for invalid file type
                        return View(model); // return view with model
                    }

                    // Generate a unique file name and save the file
                    uniqueFileName = Guid.NewGuid().ToString() + fileExtension; // create unique file name
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads"); // set uploads folder path
                    Directory.CreateDirectory(uploadsFolder); // create uploads directory if it doesn't exist
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName); // set full file path

                    using (var stream = new FileStream(filePath, FileMode.Create)) // create file stream
                    {
                        await document.CopyToAsync(stream); // copy document to stream
                    }
                }

                // Create a new claim and add it to local storage
                var claim = new ClaimForVerification // create new claim
                {
                    LecturerName = User.Identity?.Name ?? "Anonymous", // set lecturer name
                    HoursWorked = model.HoursWorked, // set hours worked
                    HourlyRate = model.HourlyRate, // set hourly rate
                    Notes = model.Notes ?? string.Empty, // set notes
                    DocumentFileName = uniqueFileName, // set document file name
                    Status = "Pending" // set status to pending
                };

                LocalClaimStorage.AddClaim(claim); // add claim to local storage

                // Redirect to ViewClaims after successful submission
                return RedirectToAction("ViewClaims"); // redirect to view claims
            }
            catch (Exception ex) // catch exceptions
            {
                // Log the error and return the view with an error message
                _logger.LogError($"Error in SubClaim POST: {ex.Message}"); // log error
                ViewBag.Message = "An error occurred while submitting your claim."; // set error message
                return View(model); // return view with model
            }
        }

        //[HttpPost] // commented out HTTP POST method

        public IActionResult VerifyClaims()
        {
            var claim = new VerifyClaimsModel
            {
                LecturerName = "Lecturer",
                HoursWorked = 5,
                HourlyRate = 50.00m,
                Notes = "worked",
                DocumentFileName = "claim.pdf",
                ClaimId = 1,
                Status = ClaimStatus.PendingVerification
            };

            return View(claim);
        }

        /*public IActionResult VerifyClaims()
        {
            try
            {
                var claims = LocalClaimStorage.GetPendingClaims(); // Fetch claims with "Pending" status
                return View(claims); // Pass the claims to the view
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in VerifyClaims: {ex.Message}");
                return RedirectToAction("Error"); // Handle errors gracefully
            }
        }

        public static List<ClaimForVerification> GetPendingClaims()
        {
            return Claims.Where(c => c.Status == "Pending").ToList();
        }*/


        [HttpPost] // HTTP POST method
        // Action for rejecting a claim
        public IActionResult RejectClaim(int id)
        {
            try
            {
                LocalClaimStorage.UpdateClaimStatus(id, "Rejected");
                return RedirectToAction("VerifyClaims");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error rejecting claim: {ex.Message}");
                return RedirectToAction("Error");
            }
        }

        /*public IActionResult ViewPendingClaims()
        {
            var pendingClaims = LocalClaimStorage.GetPendingClaims(); // Correct usage
            return View(pendingClaims);
        }*/

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] // response cache settings
        public IActionResult Error() // action for error view
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }); // return error view with request id
        }
    }
}