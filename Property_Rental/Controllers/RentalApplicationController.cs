using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineRentalPropertyManagement.DTOs;
using OnlineRentalPropertyManagement.Models;
using OnlineRentalPropertyManagement.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineRentalPropertyManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RentalApplicationController : ControllerBase
    {
        private readonly IRentalApplicationService _rentalApplicationService;

        public RentalApplicationController(IRentalApplicationService rentalApplicationService)
        {
            _rentalApplicationService = rentalApplicationService;
        }

        [HttpPost("submit")]
        [Authorize(Roles = "tenant")]
        public async Task<IActionResult> SubmitRentalApplication(RentalApplicationDTO rentalApplicationDTO)
        {
            var result = await _rentalApplicationService.SubmitRentalApplicationAsync(rentalApplicationDTO);
            return result ? Ok() : BadRequest();
        }

        [HttpGet("property/{propertyId}")]
        [Authorize(Roles = "owner")]
        public async Task<ActionResult<List<RentalApplication>>> GetRentalApplicationsByPropertyId(int propertyId)
        {
            var applications = await _rentalApplicationService.GetRentalApplicationsByPropertyIdAsync(propertyId);
            return Ok(applications);
        }

        [HttpGet("tenant/{tenantId}")]
        [Authorize(Roles = "tenant")]
        public async Task<ActionResult<List<RentalApplication>>> GetRentalApplicationsByTenantId(int tenantId)
        {
            var applications = await _rentalApplicationService.GetRentalApplicationsByTenantIdAsync(tenantId);
            return Ok(applications);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "tenant,owner")]
        public async Task<ActionResult<RentalApplication>> GetRentalApplicationById(int id)
        {
            var application = await _rentalApplicationService.GetRentalApplicationByIdAsync(id);
            return application != null ? Ok(application) : NotFound();
        }

        [HttpPut("status/{id}")]
        [Authorize(Roles = "owner,admin")]
        public async Task<IActionResult> UpdateRentalApplicationStatus(int id, [FromBody] string status)
        {
            var result = await _rentalApplicationService.UpdateRentalApplicationStatusAsync(id, status);
            return result ? Ok() : NotFound();
        }
    }
}