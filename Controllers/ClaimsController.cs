using CMCS.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class ClaimsController : Controller
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<ClaimsController> _logger;
    private const int MaxFileSize = 10 * 1024 * 1024; // 10 MB
    private static readonly string[] AllowedFileTypes = { ".pdf", ".docx", ".xlsx" };

    public ClaimsController(IWebHostEnvironment webHostEnvironment, ILogger<ClaimsController> logger)
    {
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult SubClaim()
    {
        return View("SubClaim");
    }

    [HttpPost]
    public async Task<IActionResult> SubClaim(SubClaimModel model, IFormFile document)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View("SubClaim", model);
            }

            string uniqueFileName = null;

            if (document != null)
            {
                if (document.Length > MaxFileSize)
                {
                    ModelState.AddModelError("Document", "File size must be less than 10MB");
                    return View("SubClaim", model);
                }

                var fileExtension = Path.GetExtension(document.FileName).ToLowerInvariant();
                if (!AllowedFileTypes.Contains(fileExtension))
                {
                    ModelState.AddModelError("Document", "Only PDF, DOCX, and XLSX files are allowed");
                    return View("SubClaim", model);
                }

                uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await document.CopyToAsync(stream);
                }
            }

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
            _logger.LogInformation($"Claims count after submission: {LocalClaimStorage.GetAllClaims().Count}");

            return RedirectToAction("ViewClaims");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in SubClaim POST: {ex.Message}");
            ViewBag.Message = "An error occurred while submitting your claim.";
            return View("SubClaim", model);
        }
    }

    public IActionResult ViewClaims()
    {
        var claims = LocalClaimStorage.GetAllClaims();
        return View("ViewClaims", claims);
    }

    public IActionResult ApprovedClaims()
    {
        var approvedClaims = LocalClaimStorage.GetApprovedClaims();
        return View("ApprovedClaims", approvedClaims);
    }

    public IActionResult VerifyClaims()
    {
        var claimsToVerify = LocalClaimStorage.GetClaimsToVerify();
        return View("VerifyClaims", claimsToVerify);
    }

    [HttpPost]
    public IActionResult ApproveClaim(int id)
    {
        var claim = LocalClaimStorage.GetClaimById(id);
        if (claim == null)
        {
            // Handle the case where the claim is not found
            _logger.LogWarning($"Claim with ID {id} not found.");
            return RedirectToAction("VerifyClaims");
        }

        claim.Status = "Approved";
        LocalClaimStorage.UpdateClaim(claim);
        _logger.LogInformation($"Claim with ID {id} approved.");

        return RedirectToAction("VerifyClaims");
    }

    public IActionResult UploadDoc()
    {
        return View("UploadDoc");
    }
}
