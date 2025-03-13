using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineRentalPropertyManagement.Services;

[ApiController]
[Route("api/[controller]")]
public class PropertyController : ControllerBase
{
    private readonly IPropertyService _propertyService;

    public PropertyController(IPropertyService propertyService)
    {
        _propertyService = propertyService;
    }

    [HttpGet]
    [Authorize(Roles = "tenant,owner,admin")]
    public async Task<IActionResult> GetAllProperties()
    {
        var properties = await _propertyService.GetAllPropertiesAsync();
        return Ok(properties);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "tenant,owner,admin")]
    public async Task<IActionResult> GetPropertyById(int id)
    {
        var property = await _propertyService.GetPropertyByIdAsync(id);
        if (property == null)
        {
            return NotFound("Property not found.");
        }
        return Ok(property);
    }

    [HttpPost]
    [Authorize(Roles = "owner")]
    public async Task<IActionResult> AddProperty(PropertyDTO propertyDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _propertyService.AddPropertyAsync(propertyDTO);
        if (result)
        {
            return Ok("Property added successfully.");
        }
        return BadRequest("Failed to add property.");
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "owner")]
    public async Task<IActionResult> UpdateProperty(int id, PropertyDTO propertyDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _propertyService.UpdatePropertyAsync(id, propertyDTO);
        if (result)
        {
            return Ok("Property updated successfully.");
        }
        return NotFound("Property not found.");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "owner")]
    public async Task<IActionResult> DeleteProperty(int id)
    {
        var result = await _propertyService.DeletePropertyAsync(id);
        if (result)
        {
            return Ok("Property deleted successfully.");
        }
        return NotFound("Property not found.");
    }

    [HttpGet("search")]
    [Authorize(Roles = "tenant,owner,admin")]
    public async Task<IActionResult> SearchProperties(string state, string country)
    {
        var properties = await _propertyService.SearchPropertiesAsync(state, country);
        return Ok(properties);
    }

    [HttpGet("my-properties")]
    [Authorize(Roles = "owner")]
    public async Task<IActionResult> GetPropertiesByOwnerId()
    {
        if (User.FindFirst("OwnerID") == null || !int.TryParse(User.FindFirst("OwnerID").Value, out int ownerId))
        {
            return BadRequest("Invalid owner ID.");
        }

        var properties = await _propertyService.GetPropertiesByOwnerIdAsync(ownerId);
        if (properties == null || properties.Count == 0)
        {
            return NotFound("No properties found for the owner.");
        }
        return Ok(properties);
    }
}