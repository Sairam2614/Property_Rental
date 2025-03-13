using Microsoft.EntityFrameworkCore;
using OnlineRentalPropertyManagement.Data;
using OnlineRentalPropertyManagement.Models;
using OnlineRentalPropertyManagement.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineRentalPropertyManagement.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly ApplicationDbContext _context;

        public OwnerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterOwnerAsync(Owner owner)
        {
            _context.Owners.Add(owner);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Owner> GetOwnerByEmailAsync(string email)
        {
            return await _context.Owners
                .FirstOrDefaultAsync(o => o.Email == email);
        }

        public async Task<List<LeaseAgreement>> GetLeaseAgreementsByOwnerIdAsync(int ownerId)
        {
            return await _context.LeaseAgreements
                .Include(la => la.Property)
                .Where(la => la.Property.OwnerID == ownerId)
                .ToListAsync();
        }

        public async Task<List<MaintenanceRequest>> GetMaintenanceRequestsByOwnerIdAsync(int ownerId)
        {
            return await _context.MaintenanceRequests
                .Include(mr => mr.Property)
                .Where(mr => mr.Property.OwnerID == ownerId)
                .ToListAsync();
        }

        public async Task<LeaseAgreement> GetLeaseAgreementByIdAsync(int leaseId)
        {
            return await _context.LeaseAgreements
                .Include(la => la.Property)
                .Include(la => la.Tenant)
                .FirstOrDefaultAsync(la => la.LeaseID == leaseId);
        }

        public async Task UpdateLeaseAgreementAsync(LeaseAgreement leaseAgreement)
        {
            _context.Entry(leaseAgreement).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}