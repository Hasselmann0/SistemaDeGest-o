using SistemaDeGestao.APP.DTOs.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaDeGestao.APP.Interfaces
{
    public interface IResquestService
    {
        Task<RequestDto> CreateAsync(CreateRequestDto dto, Guid userId);
        Task<RequestDetailDto?> GetByIdAsync(Guid id, Guid userId, bool isManager);
        Task<IEnumerable<RequestDto>> GetAllAsync(RequestFilterDto filter, Guid userId, bool isManager);
        Task<RequestDetailDto?> ApproveAsync(Guid requestId, ApproveRequestDto dto, Guid managerId);
        Task<RequestDetailDto?> RejectAsync(Guid requestId, RejectRequestDto dto, Guid managerId);

    }
}
