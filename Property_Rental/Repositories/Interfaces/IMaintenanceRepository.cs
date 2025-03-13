using OnlineRentalPropertyManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineRentalPropertyManagement.Repositories.Interfaces
{
    public interface IMaintenanceRepository
    {
        Task<IEnumerable<MaintenanceRequest>> GetAllAsync();
        Task<MaintenanceRequest> GetByIdAsync(int id);
        Task<MaintenanceRequest> SubmitMaintenanceRequest(MaintenanceRequest maintenanceRequest);
        Task<bool> UpdateAsync(MaintenanceRequest maintenanceRequest);
        Task<bool> DeleteAsync(int id);
    }
}