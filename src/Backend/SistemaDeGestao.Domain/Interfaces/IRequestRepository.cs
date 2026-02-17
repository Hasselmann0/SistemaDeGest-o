using SistemaDeGestao.Domain.Entities;
using SistemaDeGestao.Domain.Enums;

namespace SistemaDeGestao.Domain.Interfaces
{
    public interface IRequestRepository
    {
        Task<RequestEntity?> GetByIdAsync(Guid id);
        Task<RequestEntity?> GetByIdWithHistoryAsync(Guid id);
        Task<IEnumerable<RequestEntity>> GetAllAsync(
            RequestStatus? status = null,
            RequestCategory? category = null,
            RequestPriority? priority = null,
            string? searchText = null,
            string? userId = null);
        Task<RequestEntity> CreateAsync(RequestEntity request);
        Task<RequestEntity> UpdateAsync(RequestEntity request);
        Task AddStatusHistoryAsync(RequestStatusHistory history);
        Task<IEnumerable<RequestStatusHistory>> GetHistoryByRequestIdAsync(Guid requestId);
    }
}
