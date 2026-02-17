using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaDeGestao.APP.DTOs.Requests
{
    public record RequestDto(
    Guid Id,
    string Title,
    string Description,
    string Category,
    string Priority,
    string Status,
    Guid CreatedByUserId,
    string CreatedByUserName,
    DateTime CreatedAt,
    DateTime? UpdatedAt
    );
}
