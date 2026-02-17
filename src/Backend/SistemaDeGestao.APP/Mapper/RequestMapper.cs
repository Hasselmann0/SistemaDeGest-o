using SistemaDeGestao.APP.DTOs.Requests;
using SistemaDeGestao.Domain.Entities;

namespace SistemaDeGestao.APP.Map
{
    public static class RequestMapper
    {
        public static RequestDto ToDto(RequestEntity entity)
        {
            return new RequestDto(
                entity.Id,
                entity.Title,
                entity.Description,
                entity.Category.ToString(),
                entity.Priority.ToString(),
                entity.Status.ToString(),
                Guid.Parse(entity.CreatedByUserId),
                entity.CreatedByUser?.UserName ?? "Desconhecido",
                entity.CreatedAt,
                entity.UpdatedAt
            );
        }

        public static RequestDetailDto ToDetailDto(RequestEntity entity)
        {
            var history = entity.StatusHistories
                .OrderByDescending(h => h.CreatedAt)
                .Select(ToHistoryDto);

            return new RequestDetailDto(
                entity.Id,
                entity.Title,
                entity.Description,
                entity.Category.ToString(),
                entity.Priority.ToString(),
                entity.Status.ToString(),
                Guid.Parse(entity.CreatedByUserId),
                entity.CreatedByUser?.UserName ?? "Desconhecido",
                entity.CreatedAt,
                entity.UpdatedAt,
                history
            );
        }

        public static RequestStatusHistoryDto ToHistoryDto(RequestStatusHistory history)
        {
            return new RequestStatusHistoryDto(
                history.Id,
                history.FromStatus.ToString(),
                history.ToStatus.ToString(),
                Guid.Parse(history.ChangedByUserId),
                history.ChangedByUser?.UserName ?? "Desconhecido",
                history.Comment,
                history.CreatedAt
            );
        }
    }
}
