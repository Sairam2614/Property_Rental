using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineRentalPropertyManagement.DTOs;
using OnlineRentalPropertyManagement.Models;
using OnlineRentalPropertyManagement.Services;
using OnlineRentalPropertyManagement.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class TenantController : ControllerBase
{
    private readonly ITenantService _tenantService;
    private readonly INotificationService _notificationService;

    public TenantController(
        ITenantService tenantService,
        INotificationService notificationService)
    {
        _tenantService = tenantService;
        _notificationService = notificationService;
    }

    // Register a new tenant
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] TenantRegistrationDTO tenantRegistrationDTO)
    {
        var result = await _tenantService.RegisterTenantAsync(tenantRegistrationDTO);
        if (result)
        {
            return Ok("Tenant registered successfully.");
        }
        return BadRequest("Registration failed.");
    }

    // Tenant login
    [HttpPost("login")]
    [Authorize(Roles = "tenant")]
    public async Task<IActionResult> Login([FromBody] TenantLoginDTO tenantLoginDTO)
    {
        var token = await _tenantService.LoginTenantAsync(tenantLoginDTO);
        if (token == null)
        {
            return Unauthorized("Invalid email or password.");
        }
        return Ok(new { Token = token });
    }

    // Get all notifications for the tenant
    [HttpGet("notifications")]
    [Authorize(Roles = "tenant")]
    public async Task<IActionResult> GetNotifications()
    {
        var tenantId = int.Parse(User.FindFirst("sub")?.Value); // Get tenant ID from the token
        var notifications = await _notificationService.GetUnreadNotificationsAsync(tenantId);
        return Ok(notifications);
    }

    // Get lease agreements for the tenant
    [HttpGet("lease-agreements")]
    [Authorize(Roles = "tenant")]
    public async Task<IActionResult> GetLeaseAgreements()
    {
        var tenantId = int.Parse(User.FindFirst("sub")?.Value); // Get tenant ID from the token
        var leaseAgreements = await _tenantService.GetLeaseAgreementsAsync(tenantId);
        return Ok(leaseAgreements);
    }

    // Get maintenance requests for the tenant
    [HttpGet("maintenance-requests")]
    [Authorize(Roles = "tenant")]
    public async Task<IActionResult> GetMaintenanceRequests()
    {
        var tenantId = int.Parse(User.FindFirst("sub")?.Value); // Get tenant ID from the token
        var maintenanceRequests = await _tenantService.GetMaintenanceRequestsAsync(tenantId);
        return Ok(maintenanceRequests);
    }
}