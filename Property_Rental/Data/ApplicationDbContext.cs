using Microsoft.EntityFrameworkCore;
using OnlineRentalPropertyManagement.Models;

using Microsoft.EntityFrameworkCore;
using OnlineRentalPropertyManagement.Models;
using OnlineRentalPropertyManagement.Models.OnlineRentalPropertyManagement.Models;

namespace OnlineRentalPropertyManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }
        public DbSet<Servicerequest> MaintenanceServices { get; set; }
        public DbSet<LeaseAgreement> LeaseAgreements { get; set; }
        public DbSet<OwnerDocument> OwnerDocuments { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<RentalApplication> RentalApplications { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<MaintenanceNotification> MaintenanceNotifications { get; set; }
        public DbSet<LatePaymentNotification> LatePaymentNotifications { get; set; }
        
       // public DbSet<RentalApplicationNotification> RentalApplicationNotifications { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure primary key for LeaseAgreement
            modelBuilder.Entity<LeaseAgreement>()
                .HasKey(la => la.LeaseID); // Explicitly define LeaseID as the primary key

            // Configure relationships for Property
            modelBuilder.Entity<Property>()
                .HasOne(p => p.Owner)
                .WithMany(o => o.Properties)
                .HasForeignKey(p => p.OwnerID)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure relationships for RentalApplication
            modelBuilder.Entity<RentalApplication>()
                .HasOne(ra => ra.Property)
                .WithMany(p => p.RentalApplications)
                .HasForeignKey(ra => ra.PropertyID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RentalApplication>()
                .HasOne(ra => ra.Tenant)
                .WithMany(t => t.RentalApplications)
                .HasForeignKey(ra => ra.TenantID)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure relationships for LeaseAgreement
            modelBuilder.Entity<LeaseAgreement>()
                .HasOne(la => la.Property)
                .WithMany(p => p.LeaseAgreements)
                .HasForeignKey(la => la.PropertyID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LeaseAgreement>()
                .HasOne(la => la.Tenant)
                .WithMany(t => t.LeaseAgreements)
                .HasForeignKey(la => la.TenantID)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure relationships for MaintenanceRequest
            modelBuilder.Entity<MaintenanceRequest>()
                .HasOne(mr => mr.Property)
                .WithMany(p => p.MaintenanceRequests)
                .HasForeignKey(mr => mr.PropertyID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MaintenanceRequest>()
                .HasOne(mr => mr.Tenant)
                .WithMany(t => t.MaintenanceRequests)
                .HasForeignKey(mr => mr.TenantID)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure relationships for Servicerequest
            modelBuilder.Entity<Servicerequest>()
                .Property(ms => ms.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (ServiceStatus)Enum.Parse(typeof(ServiceStatus), v))
                .HasMaxLength(20);

            modelBuilder.Entity<Servicerequest>()
                .HasCheckConstraint("CK_Status", "[Status] IN ('Pending', 'InProgress', 'Completed')");

            modelBuilder.Entity<Servicerequest>()
                .HasOne(ms => ms.Request)
                .WithMany()
                .HasForeignKey(ms => ms.RequestID);

            // Configure one-to-one relationship between LeaseAgreement and OwnerDocument
            modelBuilder.Entity<LeaseAgreement>()
                .HasOne(la => la.OwnerDocument)
                .WithOne(od => od.LeaseAgreement)
                .HasForeignKey<OwnerDocument>(od => od.LeaseID);

            // Configure value generation for Servicerequest
            modelBuilder.Entity<Servicerequest>()
                .Property(sr => sr.ServiceID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Servicerequest>()
                .Property(sr => sr.AgentID)
                .ValueGeneratedNever();
            modelBuilder.Entity<MaintenanceNotification>()
            .HasKey(n => n.NotificationID);
            

            modelBuilder.Entity<LatePaymentNotification>()
           .HasKey(n => n.NotificationID);
            
            //modelBuilder.Entity<RentalApplicationNotification>()
            //    .HasOne(ra=>ra.TenantID)
            //    .WithOne(rn=>rn.RentalApplicationNotification)
        }
    }
    }


