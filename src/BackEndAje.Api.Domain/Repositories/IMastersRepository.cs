using BackEndAje.Api.Domain.Entities;

namespace BackEndAje.Api.Domain.Repositories
{
    public interface IMastersRepository
    {
        Task<List<ReasonRequest>> GetAllReasonRequest();

        Task<List<WithDrawalReason>> GetWithDrawalReasonsByReasonRequestId(int reasonRequestId);
    }
}