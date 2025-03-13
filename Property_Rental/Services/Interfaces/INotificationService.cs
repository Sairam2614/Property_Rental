using OnlineRentalPropertyManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineRentalPropertyManagement.Services.Interfaces
{
    public interface INotificationService
    {
        Task AddNotificationAsync(int userId, string message);
        Task<List<Notification>> GetUnreadNotificationsAsync(int userId);
        Task NotifyOwnerAsync(int propertyId, string message);
    }
}