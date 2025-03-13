using Microsoft.EntityFrameworkCore;
using OnlineRentalPropertyManagement.Data;
using OnlineRentalPropertyManagement.Models;
using OnlineRentalPropertyManagement.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineRentalPropertyManagement.Services
{
    public class RentalApplicationNotificationService : IRentalApplicationNotificationService
    {
        private readonly ApplicationDbContext _context;

        public RentalApplicationNotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddNotificationAsync(int userId, string message)
        {
            var notification = new RentalApplicationNotification
            {
                TenantID = userId,
                Message = message
            };

            _context.RentalApplicationNotifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<List<RentalApplicationNotification>> GetNotificationsAsync(int userId)
        {
            return await _context.RentalApplicationNotifications
                .Where(n => n.TenantID == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.RentalApplicationNotifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}