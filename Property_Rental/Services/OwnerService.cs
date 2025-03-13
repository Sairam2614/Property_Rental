using OnlineRentalPropertyManagement.DTOs;
using OnlineRentalPropertyManagement.Models;
using OnlineRentalPropertyManagement.Repositories.Interfaces;
using OnlineRentalPropertyManagement.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineRentalPropertyManagement.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly INotificationService _notificationService;
        private readonly IConfiguration _configuration;

        public OwnerService(
            IOwnerRepository ownerRepository,
            INotificationService notificationService,
            IConfiguration configuration)
        {
            _ownerRepository = ownerRepository;
            _notificationService = notificationService;
            _configuration = configuration;
        }

        public async Task<bool> RegisterOwnerAsync(OwnerRegistrationDTO ownerRegistrationDTO)
        {
            var owner = new Owner
            {
                Name = ownerRegistrationDTO.Name,
                Email = ownerRegistrationDTO.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(ownerRegistrationDTO.Password), // Hash the password
                ContactDetails = ownerRegistrationDTO.ContactDetails
            };

            return await _ownerRepository.RegisterOwnerAsync(owner);
        }

        public async Task<string> LoginOwnerAsync(OwnerLoginDTO ownerLoginDTO)
        {
            var owner = await _ownerRepository.GetOwnerByEmailAsync(ownerLoginDTO.Email);
            if (owner == null || !BCrypt.Net.BCrypt.Verify(ownerLoginDTO.Password, owner.Password)) // Verify hashed password
            {
                return null; // Invalid credentials
            }

            // Generate and return a JWT token
            return GenerateJwtToken(owner);
        }

        public async Task<List<LeaseAgreement>> GetLeaseAgreementsAsync(int ownerId)
        {
            return await _ownerRepository.GetLeaseAgreementsByOwnerIdAsync(ownerId);
        }

        public async Task<List<MaintenanceRequest>> GetMaintenanceRequestsAsync(int ownerId)
        {
            return await _ownerRepository.GetMaintenanceRequestsByOwnerIdAsync(ownerId);
        }

        public async Task AddOwnerDocumentsAsync(int leaseId, string ownerSignaturePath, string ownerDocumentPath)
        {
            var leaseAgreement = await _ownerRepository.GetLeaseAgreementByIdAsync(leaseId);
            if (leaseAgreement != null)
            {
                leaseAgreement.OwnerSignaturePath = ownerSignaturePath;
                leaseAgreement.OwnerDocumentPath = ownerDocumentPath;
                await _ownerRepository.UpdateLeaseAgreementAsync(leaseAgreement);

                // Notify the tenant
                var message = $"Owner has uploaded documents for lease agreement {leaseId}.";
                await _notificationService.AddNotificationAsync(leaseAgreement.TenantID, message);
            }
        }

        private string GenerateJwtToken(Owner owner)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, owner.OwnerID.ToString()),
                    new Claim(ClaimTypes.Email, owner.Email),
                    new Claim(ClaimTypes.Role, "owner")
                }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}