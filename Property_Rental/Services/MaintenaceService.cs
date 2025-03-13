using OnlineRentalPropertyManagement.Models;
using OnlineRentalPropertyManagement.Repositories.Interfaces;
using OnlineRentalPropertyManagement.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineRentalPropertyManagement.Services
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly IMaintenanceRepository _maintenanceRepository;

        public MaintenanceService(IMaintenanceRepository maintenanceRepository)
        {
            _maintenanceRepository = maintenanceRepository;
        }

        // Submit a new maintenance request
        public async Task<int> SubmitMaintenanceRequest(int tenantId, int propertyId, string issueDescription)
        {
            // Create a MaintenanceRequestDTO
            var maintenanceRequestDTO = new MaintenanceRequestDTO
            {
                PropertyID = propertyId,
                TenantID = tenantId,
                IssueDescription = issueDescription,
                Status = "Pending", // Default status
                AssignedDate = DateTime.UtcNow // Set the assigned date to current time
            };

            // Map the DTO to a MaintenanceRequest entity
            var maintenanceRequest = new MaintenanceRequest
            {
                PropertyID = maintenanceRequestDTO.PropertyID,
                TenantID = maintenanceRequestDTO.TenantID,
                IssueDescription = maintenanceRequestDTO.IssueDescription,
                Status = maintenanceRequestDTO.Status,
                AssignedDate = maintenanceRequestDTO.AssignedDate
            };

            // Pass the entity to the repository
            var createdRequest = await _maintenanceRepository.SubmitMaintenanceRequest(maintenanceRequest);
            return createdRequest.RequestID;
        }

        // Get all pending maintenance requests for a specific owner
        public async Task<List<MaintenanceRequest>> GetPendingRequestsForOwner(int ownerId)
        {
            var allRequests = await _maintenanceRepository.GetAllAsync();
            var pendingRequests = new List<MaintenanceRequest>();

            foreach (var request in allRequests)
            {
                if (request.Property.OwnerID == ownerId && request.Status == "Pending")
                {
                    pendingRequests.Add(request);
                }
            }

            return pendingRequests;
        }

        // Update the status of a maintenance request
        public async Task<bool> UpdateRequestStatus(int requestId, string status)
        {
            var existingRequest = await _maintenanceRepository.GetByIdAsync(requestId);
            if (existingRequest == null)
            {
                return false;
            }

            existingRequest.Status = status;
            return await _maintenanceRepository.UpdateAsync(existingRequest);
        }

        // Generate a bill for a service request
        public async Task<bool> GenerateBill(int serviceRequestId, double amount)
        {
            // This method would typically interact with a billing service or repository
            // For now, we'll just return true as a placeholder
            return await Task.FromResult(true);
        }

        // Get all maintenance requests
        public async Task<IEnumerable<MaintenanceRequest>> GetAllMaintenanceRequestsAsync()
        {
            return await _maintenanceRepository.GetAllAsync();
        }

        // Get a maintenance request by ID
        public async Task<MaintenanceRequest> GetMaintenanceRequestByIdAsync(int id)
        {
            return await _maintenanceRepository.GetByIdAsync(id);
        }

        // Create a new maintenance request
        public async Task<MaintenanceRequest> CreateMaintenanceRequestAsync(MaintenanceRequestDTO maintenanceRequestDTO)
        {
            // Map the DTO to a MaintenanceRequest entity
            var maintenanceRequest = new MaintenanceRequest
            {
                PropertyID = maintenanceRequestDTO.PropertyID,
                TenantID = maintenanceRequestDTO.TenantID,
                IssueDescription = maintenanceRequestDTO.IssueDescription,
                Status = maintenanceRequestDTO.Status,
                AssignedDate = maintenanceRequestDTO.AssignedDate
            };

            // Pass the entity to the repository
            return await _maintenanceRepository.SubmitMaintenanceRequest(maintenanceRequest);
        }

        // Update an existing maintenance request
        public async Task<bool> UpdateMaintenanceRequestAsync(int id, MaintenanceRequestDTO maintenanceRequestDTO)
        {
            // Fetch the existing request
            var existingRequest = await _maintenanceRepository.GetByIdAsync(id);
            if (existingRequest == null)
            {
                return false;
            }

            // Map the DTO to the existing entity
            existingRequest.PropertyID = maintenanceRequestDTO.PropertyID;
            existingRequest.TenantID = maintenanceRequestDTO.TenantID;
            existingRequest.IssueDescription = maintenanceRequestDTO.IssueDescription;
            existingRequest.Status = maintenanceRequestDTO.Status;
            existingRequest.AssignedDate = maintenanceRequestDTO.AssignedDate;

            // Call the repository's UpdateAsync method
            return await _maintenanceRepository.UpdateAsync(existingRequest);
        }

        // Delete a maintenance request
        public async Task<bool> DeleteMaintenanceRequestAsync(int id)
        {
            return await _maintenanceRepository.DeleteAsync(id);
        }
    }
}