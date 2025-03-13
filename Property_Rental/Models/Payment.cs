namespace OnlineRentalPropertyManagement.Models
{
    public class Payment
    {
        public int PaymentID { get; set; } // Primary Key
        public int LeaseID { get; set; }   // Foreign Key to Lease
        //public int TenantID { get; set; }  // Foreign Key to Tenant
        //public int PropertyID { get; set; } // Foreign Key to Property
        public double Amount { get; set; } // Payment amount
        public PaymentMethod PaymentMethod { get; set; } // Payment method (e.g., Credit Card, Debit Card, PayPal)
        public DateTime PaymentDate { get; set; } // Date of payment
        public Status Status { get; set; } // Payment status (e.g., Paid, Pending, Failed)
        public LeaseAgreement LeaseAgreement { get; set; } // Links to the LeaseAgreement table

        //// Navigation Properties
        //public Tenant Tenant { get; set; } // Links to the Tenant table
        //public Property Property { get; set; } // Links to the Property table
    }
    public enum Status
    {
        Paid,
        Pending,
        Failed
    }
    public enum PaymentMethod
    {
        CreditCard,
        DebitCard,
        PayPal
    }
}
