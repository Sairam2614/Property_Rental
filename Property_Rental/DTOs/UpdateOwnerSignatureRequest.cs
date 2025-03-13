namespace OnlineRentalPropertyManagement.DTOs
{
    public class UpdateOwnerSignatureRequest
    {
        public IFormFile OwnerSignatureFile { get; set; } // Owner's signature file
        public IFormFile OwnerDocumentFile { get; set; } // Owner's document file
    }
}
