using Microsoft.EntityFrameworkCore;
using OnlineRentalPropertyManagement.Data;
using OnlineRentalPropertyManagement.Models;
using OnlineRentalPropertyManagement.Models.OnlineRentalPropertyManagement.Models;
using OnlineRentalPropertyManagement.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineRentalPropertyManagement.Services
{
    public class LatePaymentNotificationService : ILatePaymentNotificationService
    {
        private readonly ApplicationDbContext _context;

        public LatePaymentNotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddNotificationAsync(int userId, string message)
        {
            var notification = new LatePaymentNotification
            {
                UserID = userId,
                Message = message
            };

            _context.LatePaymentNotifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<List<LatePaymentNotification>> GetNotificationsAsync(int userId)
        {
            return await _context.LatePaymentNotifications
                .Where(n => n.UserID == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.LatePaymentNotifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}