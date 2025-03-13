using OnlineRentalPropertyManagement.Data;
using OnlineRentalPropertyManagement.Models;
using OnlineRentalPropertyManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OnlineRentalPropertyManagement.Services
{
    public class LeaseAgreementService : ILeaseAgreementService
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public LeaseAgreementService(
            ApplicationDbContext context,
            INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        // Create a new lease agreement (tenant submits)
        public async Task<LeaseAgreement> CreateLeaseAgreementAsync(LeaseAgreement leaseAgreement)
        {
            var property = await _context.Properties.FindAsync(leaseAgreement.PropertyID);
            var tenant = await _context.Tenants.FindAsync(leaseAgreement.TenantID);

            if (property == null || tenant == null)
            {
                throw new Exception("Property or Tenant not found.");
            }

            _context.LeaseAgreements.Add(leaseAgreement);
            await _context.SaveChangesAsync();

            var message = $"A new lease agreement has been submitted by Tenant ID {leaseAgreement.TenantID} for Property ID {leaseAgreement.PropertyID}.";
            await _notificationService.AddNotificationAsync(property.OwnerID, message);

            return leaseAgreement;
        }

        // Add owner documents (owner submits signature and documents)
        public async Task<OwnerDocument> AddOwnerDocumentsAsync(int leaseID, string ownerSignaturePath, string ownerDocumentPath)
        {
            var leaseAgreement = await _context.LeaseAgreements
                .FirstOrDefaultAsync(la => la.LeaseID == leaseID);

            if (leaseAgreement == null)
            {
                throw new Exception($"Lease agreement with ID {leaseID} not found.");
            }

            var ownerDocument = new OwnerDocument
            {
                LeaseID = leaseID,
                OwnerSignaturePath = ownerSignaturePath,
                OwnerDocumentPath = ownerDocumentPath
            };

            _context.OwnerDocuments.Add(ownerDocument);
            await _context.SaveChangesAsync();

            return ownerDocument;
        }

        // Generate a legal document for the lease agreement
        public async Task<string> GenerateLegalDocumentAsync(int leaseID)
        {
            var leaseAgreement = await _context.LeaseAgreements
                .Include(la => la.Property)
                .Include(la => la.Tenant)
                .FirstOrDefaultAsync(la => la.LeaseID == leaseID);

            if (leaseAgreement == null)
            {
                throw new Exception("Lease agreement not found.");
            }

            // Generate a legal document (this is a placeholder implementation)
            var documentContent = $"Legal Document for Lease ID: {leaseID}\n" +
                                 $"Property ID: {leaseAgreement.PropertyID}\n" +
                                 $"Tenant ID: {leaseAgreement.TenantID}\n" +
                                 $"Start Date: {leaseAgreement.StartDate}\n" +
                                 $"End Date: {leaseAgreement.EndDate}\n" +
                                 $"Status: {leaseAgreement.Status}";

            // Define the directory path
            var documentsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "LegalDocuments");

            // Create the directory if it doesn't exist
            if (!Directory.Exists(documentsDirectory))
            {
                Directory.CreateDirectory(documentsDirectory);
            }

            // Define the file path
            var documentPath = Path.Combine(documentsDirectory, $"lease_{leaseID}.txt");

            // Write the document content to the file
            await File.WriteAllTextAsync(documentPath, documentContent);

            return documentPath;
        }

        // Execute the lease agreement (mark as executed)
        public async Task<LeaseAgreement> ExecuteLeaseAsync(int leaseID)
        {
            var leaseAgreement = await _context.LeaseAgreements.FindAsync(leaseID);

            if (leaseAgreement == null)
            {
                throw new Exception($"Lease agreement with ID {leaseID} not found.");
            }

            leaseAgreement.Status = "Executed";
            _context.LeaseAgreements.Update(leaseAgreement);
            await _context.SaveChangesAsync();

            return leaseAgreement;
        }
    }
}
