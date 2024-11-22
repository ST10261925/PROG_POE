namespace CMCS.Models // namespace for CMCS models
{
    public static class LocalClaimStorage // static class for managing claims in local storage
    {
        private static List<ClaimForVerification> claims = new List<ClaimForVerification>(); // list to store claims

        // method to retrieve a claim by its identifier
        public static ClaimForVerification? GetClaimById(int id)
        {
            return claims.FirstOrDefault(c => c.ClaimId == id); // returns the first claim matching the given id or null if not found
        }

        // method to update an existing claim
        public static void UpdateClaim(ClaimForVerification updatedClaim)
        {
            var existingClaim = GetClaimById(updatedClaim.ClaimId); // find the existing claim by id
            if (existingClaim != null) // if the claim exists
            {
                // update properties of the existing claim with values from the updated claim
                existingClaim.HoursWorked = updatedClaim.HoursWorked;
                existingClaim.HourlyRate = updatedClaim.HourlyRate;
                existingClaim.Notes = updatedClaim.Notes;
                existingClaim.DocumentFileName = updatedClaim.DocumentFileName;
                existingClaim.Status = updatedClaim.Status;
            }
        }

        private static List<ClaimForVerification> Claims = new List<ClaimForVerification>(); // list to store claims (duplicate list, consider removing)

        // method to add a new claim
        public static void AddClaim(ClaimForVerification claim)
        {
            Claims.Add(claim); // adds the new claim to the list
        }

        // method to retrieve all claims
        public static List<ClaimForVerification> GetAllClaims()
        {
            return Claims; // returns the list of all claims
        }

        // method to retrieve approved claims
        public static List<ClaimForVerification> GetApprovedClaims()
        {
            return Claims.Where(c => c.Status == "Approved").ToList(); // filters and returns approved claims
        }

        // method to retrieve claims that are pending verification
        public static List<ClaimForVerification> GetClaimsToVerify()
        {
            return Claims.Where(c => c.Status == "Pending").ToList(); // filters and returns claims that are pending
        }

        // method to update the status of a claim
        public static void UpdateClaimStatus(int claimId, string status)
        {
            var claim = Claims.FirstOrDefault(c => c.Id == claimId); // find the claim by id
            if (claim != null) // if the claim exists
            {
                claim.Status = status; // update the status of the claim
            }
        }
    }
}