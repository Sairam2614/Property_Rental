
    using global::OnlineRentalPropertyManagement.Models;
    using OnlineRentalPropertyManagement.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    namespace OnlineRentalPropertyManagement.Repositories.Interfaces
    {
        public interface INotificationRepository
        {
            Task AddNotificationAsync(Notification notification);
            Task<List<Notification>> GetUnreadNotificationsByUserIdAsync(int userId);
            Task MarkNotificationAsReadAsync(int notificationId);
        }
    }

