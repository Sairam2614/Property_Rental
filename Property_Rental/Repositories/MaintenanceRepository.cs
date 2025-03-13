using Microsoft.EntityFrameworkCore;
using OnlineRentalPropertyManagement.Data;
using OnlineRentalPropertyManagement.Models;
using OnlineRentalPropertyManagement.Repositories.Interfaces;
using OnlineRentalPropertyManagement.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineRentalPropertyManagement.Repositories
{
    public class MaintenanceRepository : IMaintenanceRepository
    {
        private readonly ApplicationDbContext _context;

        public MaintenanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all maintenance requests
        public async Task<IEnumerable<MaintenanceRequest>> GetAllAsync()
        {
            return await _context.MaintenanceRequests
                                .Include(mr => mr.Property) // Include related Property data
                                .Include(mr => mr.Tenant)   // Include related Tenant data
                                .ToListAsync();
        }

        // Get a maintenance request by ID
        public async Task<MaintenanceRequest> GetByIdAsync(int id)
        {
            var maintenanceRequest = await _context.MaintenanceRequests
                                                  .Include(mr => mr.Property) // Include related Property data
                                                  .Include(mr => mr.Tenant)   // Include related Tenant data
                                                  .FirstOrDefaultAsync(mr => mr.RequestID == id);

            if (maintenanceRequest == null)
            {
                throw new NotFoundException($"Maintenance request with ID {id} not found.");
            }

            return maintenanceRequest;
        }

        // Add a new maintenance request (using DTO)
        public async Task<MaintenanceRequest> SubmitMaintenanceRequest(MaintenanceRequestDTO maintenanceRequestDTO)
        {
            var maintenanceRequest = new MaintenanceRequest
            {
                PropertyID = maintenanceRequestDTO.PropertyID,
                TenantID = maintenanceRequestDTO.TenantID,
                IssueDescription = maintenanceRequestDTO.IssueDescription,
                Status = maintenanceRequestDTO.Status,
                AssignedDate = maintenanceRequestDTO.AssignedDate
            };

            _context.MaintenanceRequests.Add(maintenanceRequest);
            await _context.SaveChangesAsync();
            return maintenanceRequest;
        }

        // Add a new maintenance request (using entity)
        public async Task<MaintenanceRequest> SubmitMaintenanceRequest(MaintenanceRequest maintenanceRequest)
        {
            _context.MaintenanceRequests.Add(maintenanceRequest);
            await _context.SaveChangesAsync();
            return maintenanceRequest;
        }

        // Update an existing maintenance request
        public async Task<bool> UpdateAsync(int id, MaintenanceRequestDTO maintenanceRequestDTO)
        {
            var existingRequest = await _context.MaintenanceRequests.FindAsync(id);
            if (existingRequest == null)
            {
                throw new NotFoundException($"Maintenance request with ID {id} not found.");
            }

            // Update fields from DTO
            existingRequest.PropertyID = maintenanceRequestDTO.PropertyID;
            existingRequest.TenantID = maintenanceRequestDTO.TenantID;
            existingRequest.IssueDescription = maintenanceRequestDTO.IssueDescription;
            existingRequest.Status = maintenanceRequestDTO.Status;
            existingRequest.AssignedDate = maintenanceRequestDTO.AssignedDate;

            _context.Entry(existingRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await MaintenanceRequestExistsAsync(id))
                {
                    throw new NotFoundException($"Maintenance request with ID {id} not found.");
                }
                else
                {
                    throw;
                }
            }
        }

        // Delete a maintenance request
        public async Task<bool> DeleteAsync(int id)
        {
            var maintenanceRequest = await _context.MaintenanceRequests.FindAsync(id);
            if (maintenanceRequest == null)
            {
                throw new NotFoundException($"Maintenance request with ID {id} not found.");
            }

            _context.MaintenanceRequests.Remove(maintenanceRequest);
            await _context.SaveChangesAsync();
            return true;
        }

        // Check if a maintenance request exists
        private async Task<bool> MaintenanceRequestExistsAsync(int id)
        {
            return await _context.MaintenanceRequests.AnyAsync(e => e.RequestID == id);
        }

        // Update an existing maintenance request (using entity)
        public async Task<bool> UpdateAsync(MaintenanceRequest maintenanceRequest)
        {
            _context.Entry(maintenanceRequest).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}