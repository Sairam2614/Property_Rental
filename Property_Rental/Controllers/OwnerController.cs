using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineRentalPropertyManagement.DTOs;
using OnlineRentalPropertyManagement.Services.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class OwnerController : ControllerBase
{
    private readonly IOwnerService _ownerService;
    private readonly INotificationService _notificationService;

    public OwnerController(
        IOwnerService ownerService,
        INotificationService notificationService)
    {
        _ownerService = ownerService;
        _notificationService = notificationService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] OwnerRegistrationDTO ownerRegistrationDTO)
    {
        var result = await _ownerService.RegisterOwnerAsync(ownerRegistrationDTO);
        if (result)
        {
            return Ok(new { Success = true, Message = "Owner registered successfully." });
        }
        return BadRequest(new { Success = false, Message = "Registration failed." });
    }

    [HttpPost("login")]
    [Authorize(Roles = "owner")]
    public async Task<IActionResult> Login([FromBody] OwnerLoginDTO ownerLoginDTO)
    {
        var token = await _ownerService.LoginOwnerAsync(ownerLoginDTO);
        if (token == null)
        {
            return Unauthorized(new { Success = false, Message = "Invalid email or password." });
        }
        return Ok(new { Success = true, Token = token });
    }

    [HttpGet("notifications")]
    [Authorize(Roles = "owner")]
    public async Task<IActionResult> GetNotifications()
    {
        var ownerId = int.Parse(User.FindFirst("sub")?.Value); // Get owner ID from the token
        var notifications = await _notificationService.GetUnreadNotificationsAsync(ownerId);
        return Ok(new { Success = true, Notifications = notifications });
    }

    [HttpGet("lease-agreements")]
    [Authorize(Roles = "owner")]
    public async Task<IActionResult> GetLeaseAgreements()
    {
        var ownerId = int.Parse(User.FindFirst("sub")?.Value); // Get owner ID from the token
        var leaseAgreements = await _ownerService.GetLeaseAgreementsAsync(ownerId);
        return Ok(new { Success = true, LeaseAgreements = leaseAgreements });
    }

    [HttpGet("maintenance-requests")]
    [Authorize(Roles = "owner")]
    public async Task<IActionResult> GetMaintenanceRequests()
    {
        var ownerId = int.Parse(User.FindFirst("sub")?.Value); // Get owner ID from the token
        var maintenanceRequests = await _ownerService.GetMaintenanceRequestsAsync(ownerId);
        return Ok(new { Success = true, MaintenanceRequests = maintenanceRequests });
    }

    [HttpPut("{leaseId}/owner-documents")]
    [Authorize(Roles = "owner")]
    public async Task<IActionResult> AddOwnerDocuments(int leaseId, [FromForm] UpdateOwnerSignatureRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { Success = false, Message = "Invalid request.", Errors = ModelState.Values });
        }

        try
        {
            var ownerSignaturePath = await SaveFileAsync(request.OwnerSignatureFile);
            var ownerDocumentPath = await SaveFileAsync(request.OwnerDocumentFile);

            await _ownerService.AddOwnerDocumentsAsync(leaseId, ownerSignaturePath, ownerDocumentPath);
            return Ok(new { Success = true, Message = "Owner documents uploaded successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Success = false, Message = "An error occurred while uploading documents.", Error = ex.Message });
        }
    }

    private async Task<string> SaveFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("File is empty.");
        }

        // Validate file type and size
        var allowedExtensions = new[] { ".pdf", ".png", ".jpg", ".jpeg" };
        var fileExtension = Path.GetExtension(file.FileName).ToLower();
        if (!allowedExtensions.Contains(fileExtension))
        {
            throw new ArgumentException("Invalid file type. Allowed types: PDF, PNG, JPG.");
        }

        if (file.Length > 5 * 1024 * 1024) // 5 MB limit
        {
            throw new ArgumentException("File size exceeds the limit of 5 MB.");
        }

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return filePath;
    }
}