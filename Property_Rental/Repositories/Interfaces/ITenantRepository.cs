using OnlineRentalPropertyManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineRentalPropertyManagement.Repositories.Interfaces
{
    public interface ITenantRepository
    {
        Task<bool> RegisterTenantAsync(Tenant tenant);
        Task<Tenant> GetTenantByEmailAsync(string email);
        Task<List<LeaseAgreement>> GetLeaseAgreementsByTenantIdAsync(int tenantId);
        Task<List<MaintenanceRequest>> GetMaintenanceRequestsByTenantIdAsync(int tenantId);
    }
}