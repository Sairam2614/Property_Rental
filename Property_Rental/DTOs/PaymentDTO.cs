namespace OnlineRentalPropertyManagement.DTOs
{
    public class PaymentDto
    {
        public int PaymentID { get; set; } // Primary Key
        public int LeaseID { get; set; }   // Foreign Key to Lease
        public double Amount { get; set; } // Payment amount
        public DateTime PaymentDate { get; set; } // Date of payment
        public string Status { get; set; } // Payment status (e.g., Paid, Pending, Failed)
    }
}
