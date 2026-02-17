using SistemaDeGestao.APP.DTOs.Requests;
using SistemaDeGestao.APP.Interfaces;
using SistemaDeGestao.APP.Map;
using SistemaDeGestao.Domain.Entities;
using SistemaDeGestao.Domain.Enums;
using SistemaDeGestao.Domain.Interfaces;

namespace SistemaDeGestao.APP.Services
{
    public class RequestService : IResquestService
    {
        private readonly IRequestRepository _requestRepository;

        public RequestService(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public async Task<RequestDto> CreateAsync(CreateRequestDto dto, Guid userId)
        {
            var request = new RequestEntity
            {
                Title = dto.Title,
                Description = dto.Description,
                Category = dto.Category,
                Priority = dto.Priority,
                Status = RequestStatus.Pending,
                CreatedByUserId = userId.ToString()
            };

            var created = await _requestRepository.CreateAsync(request);

            // Registrar histórico inicial
            await _requestRepository.AddStatusHistoryAsync(new RequestStatusHistory
            {
                RequestId = created.Id,
                FromStatus = RequestStatus.Pending,
                ToStatus = RequestStatus.Pending,
                ChangedByUserId = userId.ToString(),
                Comment = "Solicitação criada"
            });

            return RequestMapper.ToDto(created);
        }

        public async Task<RequestDetailDto?> GetByIdAsync(Guid id, Guid userId, bool isManager)
        {
            var request = await _requestRepository.GetByIdWithHistoryAsync(id);
            if (request is null)
                return null;

            // User só pode ver suas próprias solicitações
            if (!isManager && request.CreatedByUserId != userId.ToString())
                return null;

            return RequestMapper.ToDetailDto(request);
        }

        public async Task<IEnumerable<RequestDto>> GetAllAsync(RequestFilterDto filter, Guid userId, bool isManager)
        {
            // User só vê as próprias, Manager vê todas
            var userIdFilter = isManager ? null : userId.ToString();

            var requests = await _requestRepository.GetAllAsync(
                filter.Status,
                filter.Category,
                filter.Priority,
                filter.SearchText,
                userIdFilter);

            return requests.Select(RequestMapper.ToDto);
        }

        public async Task<RequestDetailDto?> ApproveAsync(Guid requestId, ApproveRequestDto dto, Guid managerId)
        {
            var request = await _requestRepository.GetByIdAsync(requestId);
            if (request is null)
                return null;

            if (request.Status != RequestStatus.Pending)
                return null;

            var previousStatus = request.Status;
            request.Status = RequestStatus.Approved;

            await _requestRepository.UpdateAsync(request);

            await _requestRepository.AddStatusHistoryAsync(new RequestStatusHistory
            {
                RequestId = requestId,
                FromStatus = previousStatus,
                ToStatus = RequestStatus.Approved,
                ChangedByUserId = managerId.ToString(),
                Comment = dto.Comment
            });

            return await GetByIdAsync(requestId, managerId, true);
        }

        public async Task<RequestDetailDto?> RejectAsync(Guid requestId, RejectRequestDto dto, Guid managerId)
        {
            var request = await _requestRepository.GetByIdAsync(requestId);
            if (request is null)
                return null;

            if (request.Status != RequestStatus.Pending)
                return null;

            var previousStatus = request.Status;
            request.Status = RequestStatus.Rejected;

            await _requestRepository.UpdateAsync(request);

            // Registrar histórico
            await _requestRepository.AddStatusHistoryAsync(new RequestStatusHistory
            {
                RequestId = requestId,
                FromStatus = previousStatus,
                ToStatus = RequestStatus.Rejected,
                ChangedByUserId = managerId.ToString(),
                Comment = dto.Comment
            });

            return await GetByIdAsync(requestId, managerId, true);
        }
    }
}
