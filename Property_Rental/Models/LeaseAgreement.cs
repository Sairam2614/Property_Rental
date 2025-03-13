namespace OnlineRentalPropertyManagement.Models
{
    public class LeaseAgreement
    {
        public int LeaseID { get; set; } // Primary Key
        public int PropertyID { get; set; } // Foreign Key to Property
        public int TenantID { get; set; } // Foreign Key to Tenant
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TenantSignaturePath { get; set; } // Path to tenant's signature file
        public string TenantDocumentPath { get; set; } // Path to tenant's uploaded document
        public string Status { get; set; } // e.g., Pending, Signed, Executed

        // Navigation Properties
        public Property Property { get; set; }
        public Tenant Tenant { get; set; }
        public OwnerDocument OwnerDocument { get; set; } // Navigation property for OwnerDocument
        public ICollection<Payment> Payments { get; set; } // Navigation property for Payments

        public double RentAmount => Property?.RentAmount ?? 0;
    }
}