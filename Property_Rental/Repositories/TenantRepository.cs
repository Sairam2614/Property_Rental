using Microsoft.EntityFrameworkCore;
using OnlineRentalPropertyManagement.Data;
using OnlineRentalPropertyManagement.Models;
using OnlineRentalPropertyManagement.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineRentalPropertyManagement.Repositories
{
    public class TenantRepository : ITenantRepository
    {
        private readonly ApplicationDbContext _context;

        public TenantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterTenantAsync(Tenant tenant)
        {
            _context.Tenants.Add(tenant);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Tenant> GetTenantByEmailAsync(string email)
        {
            return await _context.Tenants
                .FirstOrDefaultAsync(t => t.Email == email);
        }

        public async Task<List<LeaseAgreement>> GetLeaseAgreementsByTenantIdAsync(int tenantId)
        {
            return await _context.LeaseAgreements
                .Where(la => la.TenantID == tenantId)
                .Include(la => la.Property)
                .ToListAsync();
        }

        public async Task<List<MaintenanceRequest>> GetMaintenanceRequestsByTenantIdAsync(int tenantId)
        {
            return await _context.MaintenanceRequests
                .Where(mr => mr.TenantID == tenantId)
                .Include(mr => mr.Property)
                .ToListAsync();
        }
    }
}