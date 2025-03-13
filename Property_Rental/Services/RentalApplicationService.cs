using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineRentalPropertyManagement.DTOs;
using OnlineRentalPropertyManagement.Models;
using OnlineRentalPropertyManagement.Repositories;

namespace OnlineRentalPropertyManagement.Services
{
    public class RentalApplicationService : IRentalApplicationService
    {
        private readonly IRentalApplicationRepository _rentalApplicationRepository;

        public RentalApplicationService(IRentalApplicationRepository rentalApplicationRepository)
        {
            _rentalApplicationRepository = rentalApplicationRepository;
        }

        public async Task<bool> SubmitRentalApplicationAsync(RentalApplicationDTO rentalApplicationDTO)
        {
            var rentalApplication = new RentalApplication
            {
                PropertyID = rentalApplicationDTO.PropertyID,
                TenantID = rentalApplicationDTO.TenantID,
                NoOfPeople = rentalApplicationDTO.NoOfPeople,
                StayPeriod = rentalApplicationDTO.StayPeriod,
                TentativeStartDate = rentalApplicationDTO.TentativeStartDate,
                PermanentAddress = rentalApplicationDTO.PermanentAddress,
                State = rentalApplicationDTO.State,
                Country = rentalApplicationDTO.Country,
                SpecificRequirements = rentalApplicationDTO.SpecificRequirements,
                ApplicationDate = DateTime.UtcNow,
                Status = "Pending"
            };

            return await _rentalApplicationRepository.AddRentalApplicationAsync(rentalApplication);
        }

        public async Task<List<RentalApplication>> GetRentalApplicationsByPropertyIdAsync(int propertyId)
        {
            return await _rentalApplicationRepository.GetRentalApplicationsByPropertyIdAsync(propertyId);
        }

        public async Task<List<RentalApplication>> GetRentalApplicationsByTenantIdAsync(int tenantId)
        {
            return await _rentalApplicationRepository.GetRentalApplicationsByTenantIdAsync(tenantId);
        }

        public async Task<RentalApplication> GetRentalApplicationByIdAsync(int id)
        {
            return await _rentalApplicationRepository.GetRentalApplicationByIdAsync(id);
        }

        public async Task<bool> UpdateRentalApplicationStatusAsync(int id, string status)
        {
            return await _rentalApplicationRepository.UpdateRentalApplicationStatusAsync(id, status);
        }
    }
}
