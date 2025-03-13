


using OnlineRentalPropertyManagement.Models;

namespace OnlineRentalPropertyManagement.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment> GetPaymentByIdAsync(int paymentId);
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        Task AddPaymentAsync(Payment payment);
        Task UpdatePaymentAsync(Payment payment);
        Task DeletePaymentAsync(int paymentId);
        Task<bool> ValidatePaymentAmountAsync(int propertyId, double amount);
    }

}