using OnlineRentalPropertyManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineRentalPropertyManagement.Repositories.Interfaces
{
    public interface IOwnerRepository
    {
        Task<bool> RegisterOwnerAsync(Owner owner);
        Task<Owner> GetOwnerByEmailAsync(string email);
        Task<List<LeaseAgreement>> GetLeaseAgreementsByOwnerIdAsync(int ownerId);
        Task<List<MaintenanceRequest>> GetMaintenanceRequestsByOwnerIdAsync(int ownerId);
        Task<LeaseAgreement> GetLeaseAgreementByIdAsync(int leaseId);
        Task UpdateLeaseAgreementAsync(LeaseAgreement leaseAgreement);
    }
}