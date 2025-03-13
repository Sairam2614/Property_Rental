using OnlineRentalPropertyManagement.Models;
using System.Threading.Tasks;

namespace OnlineRentalPropertyManagement.Services.Interfaces
{
    public interface ILeaseAgreementService
    {
        Task<LeaseAgreement> CreateLeaseAgreementAsync(LeaseAgreement leaseAgreement);
        Task<OwnerDocument> AddOwnerDocumentsAsync(int leaseID, string ownerSignaturePath, string ownerDocumentPath);
        Task<string> GenerateLegalDocumentAsync(int leaseID);
        Task<LeaseAgreement> ExecuteLeaseAsync(int leaseID);
        
    }
}