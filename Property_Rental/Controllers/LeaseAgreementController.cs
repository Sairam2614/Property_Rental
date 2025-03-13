using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineRentalPropertyManagement.Data;
using OnlineRentalPropertyManagement.DTOs;
using OnlineRentalPropertyManagement.Models;
using OnlineRentalPropertyManagement.Services.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OnlineRentalPropertyManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LeaseAgreementController : ControllerBase
    {
        private readonly ILeaseAgreementService _leaseAgreementService;
        private readonly ApplicationDbContext _context;

        public LeaseAgreementController(
            ILeaseAgreementService leaseAgreementService,
            ApplicationDbContext context)
        {
            _leaseAgreementService = leaseAgreementService;
            _context = context;
        }

        // Tenant Post: Create a new lease agreement
        [HttpPost]
        [Authorize(Roles = "tenant")]
        public async Task<IActionResult> CreateLeaseAgreement([FromBody] LeaseAgreementDTO leaseAgreementDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var leaseAgreement = new LeaseAgreement
                {
                    PropertyID = leaseAgreementDTO.PropertyID,
                    TenantID = leaseAgreementDTO.TenantID,
                    StartDate = leaseAgreementDTO.StartDate,
                    EndDate = leaseAgreementDTO.EndDate,
                    TenantSignaturePath = leaseAgreementDTO.TenantSignaturePath,
                    TenantDocumentPath = leaseAgreementDTO.TenantDocumentPath,
                    Status = "Pending" // Initial status
                };

                var createdLease = await _leaseAgreementService.CreateLeaseAgreementAsync(leaseAgreement);
                return Ok(createdLease);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Owner Put: Add owner documents (signature and document paths)
        [HttpPut("{leaseID}/owner")]
        [Authorize(Roles = "owner")]
        public async Task<IActionResult> AddOwnerDocuments(int leaseID, [FromBody] OwnerDocumentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Fetch the lease agreement
                var leaseAgreement = await _context.LeaseAgreements
                    .Include(la => la.Property)
                    .FirstOrDefaultAsync(la => la.LeaseID == leaseID);

                if (leaseAgreement == null)
                {
                    return NotFound("Lease agreement not found.");
                }

                // Get the current user's ID (owner's ID)
                var ownerId = User.FindFirst("sub")?.Value; // Assuming "sub" contains the owner's ID

                // Validate that the current user is the owner of the property
                if (leaseAgreement.Property.OwnerID.ToString() != ownerId)
                {
                    return Forbid("You are not authorized to update documents for this lease agreement.");
                }

                // Add owner documents
                var ownerDocument = await _leaseAgreementService.AddOwnerDocumentsAsync(
                    leaseID,
                    request.OwnerSignaturePath,
                    request.OwnerDocumentPath
                );

                return Ok(ownerDocument);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Get: Retrieve lease agreement details (only accessible to the owner of the property)
        [HttpGet("{leaseID}")]
        [Authorize(Roles = "owner")]
        public async Task<IActionResult> GetLeaseAgreement(int leaseID)
        {
            try
            {
                // Fetch the lease agreement
                var leaseAgreement = await _context.LeaseAgreements
                    .Include(la => la.Property)
                    .Include(la => la.Tenant)
                    .Include(la => la.OwnerDocument)
                    .FirstOrDefaultAsync(la => la.LeaseID == leaseID);

                if (leaseAgreement == null)
                {
                    return NotFound("Lease agreement not found.");
                }

                // Get the current user's ID (owner's ID)
                var ownerId = User.FindFirst("sub")?.Value; // Assuming "sub" contains the owner's ID

                // Validate that the current user is the owner of the property
                if (leaseAgreement.Property.OwnerID.ToString() != ownerId)
                {
                    return Forbid("You are not authorized to view this lease agreement.");
                }

                return Ok(leaseAgreement);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Generate a legal document for the lease agreement
        [HttpGet("{leaseID}/document")]
        [Authorize(Roles = "tenant,owner")]
        public async Task<IActionResult> GetLegalDocument(int leaseID)
        {
            try
            {
                var documentPath = await _leaseAgreementService.GenerateLegalDocumentAsync(leaseID);
                var documentContent = await System.IO.File.ReadAllTextAsync(documentPath);

                return Content(documentContent, "text/plain");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Execute: Mark the lease agreement as executed
        [HttpPut("{leaseID}/execute")]
        [Authorize(Roles = "owner")]
        public async Task<IActionResult> ExecuteLease(int leaseID)
        {
            try
            {
                var leaseAgreement = await _leaseAgreementService.ExecuteLeaseAsync(leaseID);
                return Ok(leaseAgreement);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}