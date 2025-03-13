using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace OnlineRentalPropertyManagement.Models
{
    public class Servicerequest
    {
        [Key]
        public int ServiceID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AgentID { get; set; }
        [ForeignKey("Request")]
        public int RequestID { get; set; }
        [Required]
        [StringLength(20)]
        public ServiceStatus Status { get; set; } // Renamed to ServiceStatus
        [Required]
        [Range(0, double.MaxValue)]
        public double ServiceBill { get; set; } // Added ServiceBill property
        public virtual Request Request { get; set; }
    }
    public class Request
    {
        [Key]
        public int RequestID { get; set; }
        // Other properties related to the Request entity
    }
    public enum ServiceStatus // Renamed to ServiceStatus
    {
        Pending,
        InProgress,
        Completed
    }
}
