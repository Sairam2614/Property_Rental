using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using OnlineRentalPropertyManagement.Repositories;
using System.IO;
using System.Threading.Tasks;
using OnlineRentalPropertyManagement.Models;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace OnlineRentalPropertyManagement.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IWebHostEnvironment _env;

        public PropertyService(IPropertyRepository propertyRepository, IWebHostEnvironment env)
        {
            _propertyRepository = propertyRepository;
            _env = env;
        }

        public async Task<List<Property>> GetAllPropertiesAsync()
        {
            return await _propertyRepository.GetAllPropertiesAsync();
        }

        public async Task<Property> GetPropertyByIdAsync(int id)
        {
            return await _propertyRepository.GetPropertyByIdAsync(id);
        }

        public async Task<bool> AddPropertyAsync(PropertyDTO propertyDTO)
        {
            // Handle image upload
            string imagePath = await SaveFileAsync(propertyDTO.ImageFile, "images");

            // Handle video upload
            string videoPath = await SaveFileAsync(propertyDTO.VideoFile, "videos");

            var property = new Property
            {
                PropertyName = propertyDTO.PropertyName,
                Address = propertyDTO.Address,
                State = propertyDTO.State,
                Country = propertyDTO.Country,
                RentAmount = propertyDTO.RentAmount,
                AvailabilityStatus = propertyDTO.AvailabilityStatus,
                Amenities = propertyDTO.Amenities,
                ImagePath = imagePath,
                VideoPath = videoPath,
                OwnerID = propertyDTO.OwnerID
            };

            return await _propertyRepository.AddPropertyAsync(property);
        }

        public async Task<bool> UpdatePropertyAsync(int id, PropertyDTO propertyDTO)
        {
            var property = await _propertyRepository.GetPropertyByIdAsync(id);
            if (property == null)
            {
                return false;
            }

            // Handle image upload (if a new image is provided)
            if (propertyDTO.ImageFile != null)
            {
                // Delete the old image file if it exists
                if (!string.IsNullOrEmpty(property.ImagePath))
                {
                    DeleteFile(property.ImagePath);
                }

                // Save the new image file
                property.ImagePath = await SaveFileAsync(propertyDTO.ImageFile, "images");
            }

            // Handle video upload (if a new video is provided)
            if (propertyDTO.VideoFile != null)
            {
                // Delete the old video file if it exists
                if (!string.IsNullOrEmpty(property.VideoPath))
                {
                    DeleteFile(property.VideoPath);
                }

                // Save the new video file
                property.VideoPath = await SaveFileAsync(propertyDTO.VideoFile, "videos");
            }

            // Update other fields
            property.PropertyName = propertyDTO.PropertyName;
            property.Address = propertyDTO.Address;
            property.State = propertyDTO.State;
            property.Country = propertyDTO.Country;
            property.RentAmount = propertyDTO.RentAmount;
            property.AvailabilityStatus = propertyDTO.AvailabilityStatus;
            property.Amenities = propertyDTO.Amenities;

            return await _propertyRepository.UpdatePropertyAsync(property);
        }

        public async Task<bool> DeletePropertyAsync(int id)
        {
            var property = await _propertyRepository.GetPropertyByIdAsync(id);
            if (property == null)
            {
                return false;
            }

            // Delete associated image and video files
            if (!string.IsNullOrEmpty(property.ImagePath))
            {
                DeleteFile(property.ImagePath);
            }

            if (!string.IsNullOrEmpty(property.VideoPath))
            {
                DeleteFile(property.VideoPath);
            }

            return await _propertyRepository.DeletePropertyAsync(id);
        }

        public async Task<List<Property>> SearchPropertiesAsync(string state, string country)
        {
            return await _propertyRepository.SearchPropertiesAsync(state, country);
        }

        public async Task<List<Property>> GetPropertiesByOwnerIdAsync(int ownerId)
        {
            return await _propertyRepository.GetPropertiesByOwnerIdAsync(ownerId);
        }

        private async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            // Create the folder if it doesn't exist
            var uploadsFolder = Path.Combine(_env.WebRootPath, folderName);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate a unique file name
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return the relative path
            return $"/{folderName}/{fileName}";
        }

        private void DeleteFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            var fullPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/'));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }

      
    }
}