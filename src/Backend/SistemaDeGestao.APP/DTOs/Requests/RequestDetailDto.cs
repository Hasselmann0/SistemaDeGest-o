using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaDeGestao.APP.DTOs.Requests
{
    public record RequestDetailDto(
    Guid Id,
    string Title,
    string Description,
    string Category,
    string Priority,
    string Status,
    Guid CreatedByUserId,
    string CreatedByUserName,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    IEnumerable<RequestStatusHistoryDto> StatusHistory
    );

    public record RequestStatusHistoryDto(
        Guid Id,
        string FromStatus,
        string ToStatus,
        Guid ChangedByUserId,
        string ChangedByUserName,
        string? Comment,
        DateTime ChangedAt
    );
}
