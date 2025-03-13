using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineRentalPropertyManagement.DTOs;
using OnlineRentalPropertyManagement.Models;

namespace OnlineRentalPropertyManagement.Services
{
    public interface IRentalApplicationService
    {
        Task<bool> SubmitRentalApplicationAsync(RentalApplicationDTO rentalApplicationDTO);
        Task<List<RentalApplication>> GetRentalApplicationsByPropertyIdAsync(int propertyId);
        Task<List<RentalApplication>> GetRentalApplicationsByTenantIdAsync(int tenantId);
        Task<RentalApplication> GetRentalApplicationByIdAsync(int id);
        Task<bool> UpdateRentalApplicationStatusAsync(int id, string status);
    }
}