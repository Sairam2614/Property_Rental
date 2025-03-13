

using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineRentalPropertyManagement.Models;

namespace OnlineRentalPropertyManagement.Repositories
{
    public interface IRentalApplicationRepository
    {
        Task<bool> AddRentalApplicationAsync(RentalApplication rentalApplication);
        Task<List<RentalApplication>> GetRentalApplicationsByPropertyIdAsync(int propertyId);
        Task<List<RentalApplication>> GetRentalApplicationsByTenantIdAsync(int tenantId);
        Task<RentalApplication> GetRentalApplicationByIdAsync(int id);
        Task<bool> UpdateRentalApplicationStatusAsync(int id, string status);
    }
}
