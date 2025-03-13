using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OnlineRentalPropertyManagement.Data;
using OnlineRentalPropertyManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineRentalPropertyManagement.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly ApplicationDbContext _context;

        public PropertyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Property>> GetAllPropertiesAsync()
        {
            return await _context.Properties.Include(p => p.Owner).ToListAsync();
        }

        public async Task<Property> GetPropertyByIdAsync(int id)
        {
            return await _context.Properties.Include(p => p.Owner).FirstOrDefaultAsync(p => p.PropertyID == id);
        }

        public async Task<bool> AddPropertyAsync(Property property)
        {
            _context.Properties.Add(property);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePropertyAsync(Property property)
        {
            _context.Properties.Update(property);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePropertyAsync(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property == null)
            {
                return false;
            }

            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Property>> SearchPropertiesAsync(string state, string country)
        {
            return await _context.Properties
                .Where(p => p.State == state && p.Country == country)
                .Include(p => p.Owner)
                .ToListAsync();
        }

        public async Task<List<Property>> GetPropertiesByOwnerIdAsync(int ownerId)
        {
            return await _context.Properties
                .Where(p => p.OwnerID == ownerId)
                .Include(p => p.Owner)
                .ToListAsync();
        }
    }
}

