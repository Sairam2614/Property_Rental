using OnlineRentalPropertyManagement.DTOs;
using OnlineRentalPropertyManagement.Models;
using OnlineRentalPropertyManagement.Repositories.Interfaces;
using OnlineRentalPropertyManagement.Services.Interfaces;
using System.Threading.Tasks;

namespace OnlineRentalPropertyManagement.Services
{
    public class TenantService : ITenantService
    {
        private readonly ITenantRepository _tenantRepository;

        public TenantService(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public async Task<bool> RegisterTenantAsync(TenantRegistrationDTO tenantRegistrationDTO)
        {
            var tenant = new Tenant
            {
                Name = tenantRegistrationDTO.Name,
                Email = tenantRegistrationDTO.Email,
                Password = tenantRegistrationDTO.Password, // Hash the password before saving
                ContactDetails = tenantRegistrationDTO.ContactDetails
            };

            return await _tenantRepository.RegisterTenantAsync(tenant);
        }

        public async Task<string> LoginTenantAsync(TenantLoginDTO tenantLoginDTO)
        {
            var tenant = await _tenantRepository.GetTenantByEmailAsync(tenantLoginDTO.Email);
            if (tenant == null || tenant.Password != tenantLoginDTO.Password) // Validate password
            {
                return null;
            }

            // Generate and return a JWT token
            return "generated-jwt-token";
        }

        public async Task<List<LeaseAgreement>> GetLeaseAgreementsAsync(int tenantId)
        {
            return await _tenantRepository.GetLeaseAgreementsByTenantIdAsync(tenantId);
        }

        public async Task<List<MaintenanceRequest>> GetMaintenanceRequestsAsync(int tenantId)
        {
            return await _tenantRepository.GetMaintenanceRequestsByTenantIdAsync(tenantId);
        }
    }
}