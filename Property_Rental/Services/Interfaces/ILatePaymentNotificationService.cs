using OnlineRentalPropertyManagement.Models.OnlineRentalPropertyManagement.Models;

namespace OnlineRentalPropertyManagement.Services.Interfaces
{
    public interface ILatePaymentNotificationService
    {
        Task AddNotificationAsync(int userId, string message);
        Task<List<LatePaymentNotification>> GetNotificationsAsync(int userId);
        Task MarkAsReadAsync(int notificationId);
    }
}