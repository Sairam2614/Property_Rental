using OnlineRentalPropertyManagement.DTOs;
using OnlineRentalPropertyManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineRentalPropertyManagement.Services.Interfaces
{
    public interface ITenantService
    {
        Task<bool> RegisterTenantAsync(TenantRegistrationDTO tenantRegistrationDTO);
        Task<string> LoginTenantAsync(TenantLoginDTO tenantLoginDTO);
        Task<List<LeaseAgreement>> GetLeaseAgreementsAsync(int tenantId);
        Task<List<MaintenanceRequest>> GetMaintenanceRequestsAsync(int tenantId);
    }
}