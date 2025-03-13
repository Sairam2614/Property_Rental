public class PropertyDTO
{
    public string PropertyName { get; set; }
    public string Address { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public double RentAmount { get; set; }
    public bool AvailabilityStatus { get; set; }
    public string Amenities { get; set; }
    public IFormFile ImageFile { get; set; } // For image upload
    public IFormFile VideoFile { get; set; } // For video upload
    public int OwnerID { get; set; } // Owner who is listing the property
}