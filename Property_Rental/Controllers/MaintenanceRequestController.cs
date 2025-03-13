using Microsoft.AspNetCore.Mvc;
using OnlineRentalPropertyManagement.Services;
using OnlineRentalPropertyManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace OnlineRentalPropertyManagement.Controllers
{
    [ApiController]
    [Route("api/maintenance")]
    public class MaintenanceRequestController : ControllerBase
    {
        private readonly IMaintenanceService _maintenanceService;

        public MaintenanceRequestController(IMaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService;
        }

        [HttpPost("request")]
        [Authorize(Roles = "tenant")]
        public async Task<IActionResult> SubmitMaintenanceRequest(int tenantId, int propertyId, string issueDescription)
        {
            var requestId = await _maintenanceService.SubmitMaintenanceRequest(tenantId, propertyId, issueDescription);
            return Ok(new { Message = "Request submitted successfully", RequestId = requestId });
        }

        [HttpGet("notifications/{ownerId}")]
        [Authorize(Roles = "owner")]
        public async Task<IActionResult> GetPendingRequests(int ownerId)
        {
            var requests = await _maintenanceService.GetPendingRequestsForOwner(ownerId);
            if (requests == null || !requests.Any())
            {
                return Ok(new { Message = "No pending requests found for the owner." });
            }
            return Ok(new { Message = "Pending requests retrieved successfully", Requests = requests });
        }

        [HttpPut("status")]
        [Authorize(Roles = "owner")]
        public async Task<IActionResult> UpdateStatus(int requestId, string status)
        {
            var success = await _maintenanceService.UpdateRequestStatus(requestId, status);
            return success ? Ok(new { Message = "Status updated successfully" }) : BadRequest(new { Message = "Request not found" });
        }

        [HttpPut("bill")]
        [Authorize(Roles = "owner")]
        public async Task<IActionResult> GenerateBill(int requestId, double amount)
        {
            var success = await _maintenanceService.GenerateBill(requestId, amount);
            return success ? Ok(new { Message = "Bill generated successfully" }) : BadRequest(new { Message = "Request not found" });
        }

        [HttpGet]
        [Authorize(Roles = "owner")]
        public async Task<ActionResult<IEnumerable<MaintenanceRequest>>> GetMaintenanceRequests()
        {
            var requests = await _maintenanceService.GetAllMaintenanceRequestsAsync();
            if (requests == null || !requests.Any())
            {
                return Ok(new { Message = "No maintenance requests found." });
            }
            return Ok(new { Message = "Maintenance requests retrieved successfully", Requests = requests });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "owner,tenant")]
        public async Task<ActionResult<MaintenanceRequest>> GetMaintenanceRequest(int id)
        {
            var request = await _maintenanceService.GetMaintenanceRequestByIdAsync(id);
            return request != null ? Ok(new { Message = "Maintenance request retrieved successfully", Request = request }) : NotFound(new { Message = "Maintenance request not found" });
        }

        [HttpPost]
        [Authorize(Roles = "tenant")]
        public async Task<ActionResult<MaintenanceRequest>> PostMaintenanceRequest(MaintenanceRequestDTO maintenanceRequest)
        {
            var createdRequest = await _maintenanceService.CreateMaintenanceRequestAsync(maintenanceRequest);
            return CreatedAtAction(nameof(GetMaintenanceRequest), new { id = createdRequest.RequestID }, new { Message = "Maintenance request created successfully", Request = createdRequest });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "owner, tenant")]
        public async Task<IActionResult> PutMaintenanceRequest(int id, MaintenanceRequest maintenanceRequest)
        {
            if (id != maintenanceRequest.RequestID)
            {
                return BadRequest(new { Message = "Request ID mismatch" });
            }

            // Map MaintenanceRequest to MaintenanceRequestDTO
            var maintenanceRequestDTO = new MaintenanceRequestDTO
            {
                PropertyID = maintenanceRequest.PropertyID,
                TenantID = maintenanceRequest.TenantID,
                IssueDescription = maintenanceRequest.IssueDescription,
                Status = maintenanceRequest.Status,
                AssignedDate = maintenanceRequest.AssignedDate
            };

            // Call the service method with the DTO
            var success = await _maintenanceService.UpdateMaintenanceRequestAsync(id, maintenanceRequestDTO);

            return success ? NoContent() : NotFound(new { Message = "Maintenance request not found" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "tenant")]
        public async Task<IActionResult> DeleteMaintenanceRequest(int id)
        {
            var success = await _maintenanceService.DeleteMaintenanceRequestAsync(id);
            return success ? NoContent() : NotFound(new { Message = "Maintenance request not found" });
        }
    }
}