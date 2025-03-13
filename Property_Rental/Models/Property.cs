using System.Collections.Generic;

namespace OnlineRentalPropertyManagement.Models
{
    public class Property
    {
        public int PropertyID { get; set; }
        public string PropertyName { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public double RentAmount { get; set; }
        public bool AvailabilityStatus { get; set; }
        public string Amenities { get; set; } // Comma-separated list of amenities
        public string ImagePath { get; set; } // Path to the uploaded image
        public string VideoPath { get; set; }
        // Foreign Key
        public int OwnerID { get; set; }

        // Navigation Properties
        public Owner Owner { get; set; }
        public ICollection<RentalApplication> RentalApplications { get; set; } = new List<RentalApplication>();
        public ICollection<LeaseAgreement> LeaseAgreements { get; set; } = new List<LeaseAgreement>();
        public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();
        //public ICollection<Payment> Payment { get; set; } = new List<Payment>();
    }
}