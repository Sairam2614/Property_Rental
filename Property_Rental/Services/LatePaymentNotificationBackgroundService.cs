  using global::OnlineRentalPropertyManagement.Data;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using OnlineRentalPropertyManagement.Data;
    using OnlineRentalPropertyManagement.Models;
    using OnlineRentalPropertyManagement.Services.Interfaces;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    namespace OnlineRentalPropertyManagement.Services
    {
        public class LatePaymentNotificationBackgroundService : BackgroundService
        {
            private readonly ILogger<LatePaymentNotificationBackgroundService> _logger;
            private readonly IServiceProvider _serviceProvider;

            public LatePaymentNotificationBackgroundService(
                ILogger<LatePaymentNotificationBackgroundService> logger,
                IServiceProvider serviceProvider)
            {
                _logger = logger;
                _serviceProvider = serviceProvider;
            }

            protected override async Task ExecuteAsync(CancellationToken stoppingToken)
            {
                _logger.LogInformation("Late Payment Notification Background Service is running.");

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                            var notificationService = scope.ServiceProvider.GetRequiredService<ILatePaymentNotificationService>();

                            // Get all active leases
                            var activeLeases = dbContext.LeaseAgreements
                                .Include(la => la.Tenant)
                                .Include(la => la.Property)
                                .Where(la => la.Status == "Active")
                                .ToList();

                            foreach (var lease in activeLeases)
                            {
                                // Calculate the number of 30-day intervals since the lease start date
                                var daysSinceStart = (DateTime.UtcNow - lease.StartDate).TotalDays;
                                var intervals = (int)(daysSinceStart / 30);

                                // Check if a payment has been made for each interval
                                for (int i = 1; i <= intervals; i++)
                                {
                                    var dueDate = lease.StartDate.AddDays(i * 30);

                                    // Check if any payment was made within this interval
                                    var paymentMade = dbContext.Payments
                                        .Any(p => p.LeaseID == lease.LeaseID &&
                                                  p.PaymentDate >= dueDate.AddDays(-30) &&
                                                  p.PaymentDate <= dueDate);

                                    if (!paymentMade)
                                    {
                                        // Send a late payment notification to the tenant
                                        var tenantMessage = $"Your payment for Lease ID {lease.LeaseID} (Due Date: {dueDate:yyyy-MM-dd}) is overdue. Please make the payment as soon as possible.";
                                        await notificationService.AddNotificationAsync(lease.TenantID, tenantMessage);

                                        // Send a late payment notification to the owner
                                        var ownerMessage = $"Tenant {lease.Tenant.Name} has not made the payment for Lease ID {lease.LeaseID} (Due Date: {dueDate:yyyy-MM-dd}).";
                                        await notificationService.AddNotificationAsync(lease.Property.OwnerID, ownerMessage);

                                        _logger.LogInformation($"Late payment notifications sent for Lease ID {lease.LeaseID} (Due Date: {dueDate:yyyy-MM-dd}).");
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error occurred while processing late payment notifications.");
                    }

                    // Wait for 24 hours before running again
                    await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
                }
            }
        }
    }

