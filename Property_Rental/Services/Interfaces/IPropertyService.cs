using System.Collections.Generic;
using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OnlineRentalPropertyManagement.DTOs;
using OnlineRentalPropertyManagement.Models;

namespace OnlineRentalPropertyManagement.Services
{
    public interface IPropertyService
    {
        Task<List<Property>> GetAllPropertiesAsync();
        Task<Property> GetPropertyByIdAsync(int id);
        Task<bool> AddPropertyAsync(PropertyDTO propertyDTO);
        Task<bool> UpdatePropertyAsync(int id, PropertyDTO propertyDTO);
        Task<bool> DeletePropertyAsync(int id);
        Task<List<Property>> SearchPropertiesAsync(string state, string country);
        Task<List<Property>> GetPropertiesByOwnerIdAsync(int ownerId);
    }
}