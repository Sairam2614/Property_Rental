using System.Collections.Generic;
using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OnlineRentalPropertyManagement.Models;

namespace OnlineRentalPropertyManagement.Repositories
{
    public interface IPropertyRepository
    {
        Task<List<Property>> GetAllPropertiesAsync();
        Task<Property> GetPropertyByIdAsync(int id);
        Task<bool> AddPropertyAsync(Property property);
        Task<bool> UpdatePropertyAsync(Property property);
        Task<bool> DeletePropertyAsync(int id);
        Task<List<Property>> SearchPropertiesAsync(string state, string country);
        Task<List<Property>> GetPropertiesByOwnerIdAsync(int ownerId);
    }
}

