using OnlineRentalPropertyManagement.DTOs;
using OnlineRentalPropertyManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineRentalPropertyManagement.Services.Interfaces
{
    public interface IOwnerService
    {
        Task<bool> RegisterOwnerAsync(OwnerRegistrationDTO ownerRegistrationDTO);
        Task<string> LoginOwnerAsync(OwnerLoginDTO ownerLoginDTO);
        Task<List<LeaseAgreement>> GetLeaseAgreementsAsync(int ownerId);
        Task<List<MaintenanceRequest>> GetMaintenanceRequestsAsync(int ownerId);
        Task AddOwnerDocumentsAsync(int leaseId, string ownerSignaturePath, string ownerDocumentPath);
    }
}