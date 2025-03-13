using OnlineRentalPropertyManagement.Models;
public class Owner
{
    public int OwnerID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } // Hashed password
    public string ContactDetails { get; set; }
    //public Payment Payment { get; set; } // Navigation property
    public ICollection<Property> Properties { get; set; } // Navigation property
}