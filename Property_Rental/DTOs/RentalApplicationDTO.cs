using System;

namespace OnlineRentalPropertyManagement.DTOs
{
    public class RentalApplicationDTO
    {
        public int PropertyID { get; set; }
        public int TenantID { get; set; }
        public int NoOfPeople { get; set; }
        public string StayPeriod { get; set; }
        public DateTime TentativeStartDate { get; set; }
        public string PermanentAddress { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string SpecificRequirements { get; set; }
    }
}