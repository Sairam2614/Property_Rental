using global::OnlineRentalPropertyManagement.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineRentalPropertyManagement.Repositories.Interfaces
{
    public interface ILeaseAgreementRepository
    {
        Task<LeaseAgreement> GetByIdAsync(int leaseID);
        Task<List<LeaseAgreement>> GetAllAsync();
        Task<LeaseAgreement> AddAsync(LeaseAgreement leaseAgreement);
        Task<LeaseAgreement> UpdateAsync(LeaseAgreement leaseAgreement);
    }
}
