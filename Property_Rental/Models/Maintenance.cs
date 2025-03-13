﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineRentalPropertyManagement.Models
{
    public class MaintenanceRequest
    {
        [Key]
        public int RequestID { get; set; }

        [Required]
        public int PropertyID { get; set; }

        [ForeignKey("PropertyID")]
        public virtual Property Property { get; set; }

        [Required]
        public int TenantID { get; set; }

        [ForeignKey("TenantID")]
        public Tenant Tenant { get; set; }

        [Required]
        [StringLength(500)]
        public string IssueDescription { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } // e.g., Pending, In Progress, Completed

        [Required]
        public DateTime AssignedDate { get; set; } // Added AssignedDate property

        public virtual ICollection<MaintenanceNotification> MaintenanceNotifications { get; set; } = new List<MaintenanceNotification>();
    }
}
