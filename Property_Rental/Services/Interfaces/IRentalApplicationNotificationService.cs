using OnlineRentalPropertyManagement.Models;

namespace OnlineRentalPropertyManagement.Services.Interfaces
{
    public interface IRentalApplicationNotificationService
    {
        Task AddNotificationAsync(int userId, string message);
        Task<List<RentalApplicationNotification>> GetNotificationsAsync(int userId);
        Task MarkAsReadAsync(int notificationId);
    }
}