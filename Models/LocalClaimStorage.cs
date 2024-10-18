namespace CMCS.Models
{
    public static class LocalClaimStorage
    {
        private static List<ClaimForVerification> claims = new List<ClaimForVerification>();
        // Method to get a claim by its ID (make it nullable)
        public static ClaimForVerification? GetClaimById(int id)
        {
            return claims.FirstOrDefault(c => c.ClaimId == id);
        }
        // Method to update an existing claim
        public static void UpdateClaim(ClaimForVerification updatedClaim)
        {
            var existingClaim = GetClaimById(updatedClaim.ClaimId);
            if (existingClaim != null)
            {
                existingClaim.HoursWorked = updatedClaim.HoursWorked;
                existingClaim.HourlyRate = updatedClaim.HourlyRate;
                existingClaim.Notes = updatedClaim.Notes;
                existingClaim.DocumentFileName = updatedClaim.DocumentFileName;
                existingClaim.Status = updatedClaim.Status;
            }
        }
        // In-memory storage for claims
        private static List<ClaimForVerification> Claims = new List<ClaimForVerification>();
        // Method to add a new claim
        public static void AddClaim(ClaimForVerification claim)
        {
            Claims.Add(claim);
        }
        // Method to get all claims
        public static List<ClaimForVerification> GetAllClaims()
        {
            return Claims;
        }
        // Method to get approved claims
        public static List<ClaimForVerification> GetApprovedClaims()
        {
            return Claims.Where(c => c.Status == "Approved").ToList();
        }
        // Method to get claims that need verification
        public static List<ClaimForVerification> GetClaimsToVerify()
        {
            return Claims.Where(c => c.Status == "Pending").ToList();
        }
        // Method to update claim status
        public static void UpdateClaimStatus(int claimId, string status)
        {
            var claim = Claims.FirstOrDefault(c => c.Id == claimId);
            if (claim != null)
            {
                claim.Status = status;
            }
        }
    }
}