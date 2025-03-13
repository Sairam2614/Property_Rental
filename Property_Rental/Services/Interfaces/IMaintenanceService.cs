using OnlineRentalPropertyManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineRentalPropertyManagement.Services
{
    public interface IMaintenanceService
    {
        Task<int> SubmitMaintenanceRequest(int tenantId, int propertyId, string issueDescription);
        Task<List<MaintenanceRequest>> GetPendingRequestsForOwner(int ownerId);
        Task<bool> UpdateRequestStatus(int requestId, string status);
        Task<bool> GenerateBill(int serviceRequestId, double amount);
        Task<IEnumerable<MaintenanceRequest>> GetAllMaintenanceRequestsAsync();
        Task<MaintenanceRequest> GetMaintenanceRequestByIdAsync(int id);
        Task<MaintenanceRequest> CreateMaintenanceRequestAsync(MaintenanceRequestDTO maintenanceRequestDTO);
        Task<bool> UpdateMaintenanceRequestAsync(int id, MaintenanceRequestDTO maintenanceRequestDTO);
        Task<bool> DeleteMaintenanceRequestAsync(int id);
    }
}