namespace OnlineRentalPropertyManagement.Models
{
    public class OwnerDocument
    {
        public int OwnerDocumentID { get; set; } // Primary Key
        public int LeaseID { get; set; } // Foreign Key to LeaseAgreement
        public string OwnerSignaturePath { get; set; } // Path to owner's signature file
        public string OwnerDocumentPath { get; set; } // Path to owner's uploaded document

        // Navigation property
        public LeaseAgreement LeaseAgreement { get; set; }
    }
}