using OnlineRentalPropertyManagement.Models;
public class Tenant
{
    public int TenantID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } // Hashed password
    public string ContactDetails { get; set; }
    public ICollection<RentalApplication> RentalApplications { get; set; } // Navigation property
    public ICollection<LeaseAgreement> LeaseAgreements { get; set; } = new List<LeaseAgreement>();
    //public ICollection<Payment> Payment { get; set; } = new List<Payment>();
    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();
}