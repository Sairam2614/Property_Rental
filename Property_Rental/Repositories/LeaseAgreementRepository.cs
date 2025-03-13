using Microsoft.EntityFrameworkCore;
using OnlineRentalPropertyManagement.Data;
using OnlineRentalPropertyManagement.Models;
using OnlineRentalPropertyManagement.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineRentalPropertyManagement.Repositories
{
    public class LeaseAgreementRepository : ILeaseAgreementRepository
    {
        private readonly ApplicationDbContext _context;

        public LeaseAgreementRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<LeaseAgreement> GetByIdAsync(int leaseID)
        {
            return await _context.LeaseAgreements
                .Include(l => l.Tenant)
                .Include(l => l.Property)
                .Include(l => l.OwnerDocument) // Include OwnerDocument
                .FirstOrDefaultAsync(l => l.LeaseID == leaseID);
        }

        public async Task<List<LeaseAgreement>> GetAllAsync()
        {
            return await _context.LeaseAgreements
                .Include(l => l.Tenant)
                .Include(l => l.Property)
                .Include(l => l.OwnerDocument) // Include OwnerDocument
                .ToListAsync();
        }

        public async Task<LeaseAgreement> AddAsync(LeaseAgreement leaseAgreement)
        {
            _context.LeaseAgreements.Add(leaseAgreement);
            await _context.SaveChangesAsync();
            return leaseAgreement;
        }

        public async Task<LeaseAgreement> UpdateAsync(LeaseAgreement leaseAgreement)
        {
            _context.LeaseAgreements.Update(leaseAgreement);
            await _context.SaveChangesAsync();
            return leaseAgreement;
        }
    }
}
