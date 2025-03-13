using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineRentalPropertyManagement.Models
{
    public class MaintenanceRequestDTO
    {
        [Required]
        public int PropertyID { get; set; } // ID of the property associated with the request

        [Required]
        public int TenantID { get; set; } // ID of the tenant submitting the request

        [Required]
        [StringLength(500)]
        public string IssueDescription { get; set; } // Description of the issue

        [Required]
        [StringLength(50)]
        public string Status { get; set; } // Status of the request (e.g., Pending, In Progress, Completed)

        [Required]
        public DateTime AssignedDate { get; set; } // Date when the request is assigned
    }
}