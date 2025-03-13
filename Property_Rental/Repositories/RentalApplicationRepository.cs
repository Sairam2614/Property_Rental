using Microsoft.EntityFrameworkCore;
using OnlineRentalPropertyManagement.Data;
using OnlineRentalPropertyManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineRentalPropertyManagement.Repositories
{
    public class RentalApplicationRepository : IRentalApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public RentalApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddRentalApplicationAsync(RentalApplication rentalApplication)
        {
            _context.RentalApplications.Add(rentalApplication);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<RentalApplication>> GetRentalApplicationsByPropertyIdAsync(int propertyId)
        {
            return await _context.RentalApplications
                .Where(ra => ra.PropertyID == propertyId)
                .Include(ra => ra.Tenant)
                .ToListAsync();
        }

        public async Task<List<RentalApplication>> GetRentalApplicationsByTenantIdAsync(int tenantId)
        {
            return await _context.RentalApplications
                .Where(ra => ra.TenantID == tenantId)
                .Include(ra => ra.Property)
                .ToListAsync();
        }

        public async Task<RentalApplication> GetRentalApplicationByIdAsync(int id)
        {
            return await _context.RentalApplications
                .Include(ra => ra.Property)
                .Include(ra => ra.Tenant)
                .FirstOrDefaultAsync(ra => ra.RentalApplicationID == id);
        }

        public async Task<bool> UpdateRentalApplicationStatusAsync(int id, string status)
        {
            var rentalApplication = await _context.RentalApplications.FindAsync(id);
            if (rentalApplication == null)
            {
                return false;
            }

            rentalApplication.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
