using CMCS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CMCS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private const int MaxFileSize = 10 * 1024 * 1024; // 10MB max file size
        private readonly string[] AllowedFileTypes = { ".pdf", ".docx", ".xlsx" };

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Add ViewClaims action method
        public IActionResult ViewClaims()
        {
            try
            {
                var claims = LocalClaimStorage.GetAllClaims();
                return View(claims);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ViewClaims: {ex.Message}");
                return RedirectToAction("Error");
            }
        }

        // Add ApprovedClaims action method
        public IActionResult ApprovedClaims()
        {
            try
            {
                var approvedClaims = LocalClaimStorage.GetApprovedClaims();
                return View(approvedClaims);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ApprovedClaims: {ex.Message}");
                return RedirectToAction("Error");
            }
        }

        [HttpGet] 
        public IActionResult SubClaim()
        {
            return View(new SubClaimModel());
        }

        [HttpPost]
        public async Task<IActionResult> SubClaim(SubClaimModel model, IFormFile document)
        {
            try
            {
                
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                string uniqueFileName = null;

                // Handle file upload
                if (document != null)
                {
                    // Check file size
                    if (document.Length > MaxFileSize)
                    {
                        ModelState.AddModelError("Document", "File size must be less than 10MB");
                        return View(model);
                    }

                    // Check file type
                    var fileExtension = Path.GetExtension(document.FileName).ToLowerInvariant();
                    if (!AllowedFileTypes.Contains(fileExtension))
                    {
                        ModelState.AddModelError("Document", "Only PDF, DOCX, and XLSX files are allowed");
                        return View(model);
                    }

                    // Generate a unique file name and save the file
                    uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await document.CopyToAsync(stream);
                    }
                }

                // Create a new claim and add it to local storage
                var claim = new ClaimForVerification
                {
                    LecturerName = User.Identity?.Name ?? "Anonymous",
                    HoursWorked = model.HoursWorked,
                    HourlyRate = model.HourlyRate,
                    Notes = model.Notes ?? string.Empty,
                    DocumentFileName = uniqueFileName,
                    Status = "Pending"
                };

                LocalClaimStorage.AddClaim(claim);

                // Redirect to ViewClaims after successful submission
                return RedirectToAction("ViewClaims");
            }
            catch (Exception ex)
            {
                // Log the error and return the view with an error message
                _logger.LogError($"Error in SubClaim POST: {ex.Message}");
                ViewBag.Message = "An error occurred while submitting your claim.";
                return View(model);
            }
        }

        public IActionResult VerifyClaims()
        {
            try
            {
                var model = new VerifyClaimsModel
                {
                    Claims = LocalClaimStorage.GetAllClaims()
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in VerifyClaims: {ex.Message}");
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public IActionResult ApproveClaim(int id)
        {
            try
            {
                LocalClaimStorage.UpdateClaimStatus(id, "Approved");
                return RedirectToAction("VerifyClaims");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error approving claim: {ex.Message}");
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}