using SistemaDeGestao.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaDeGestao.APP.DTOs.Requests
{
    public record RequestFilterDto(
    RequestStatus? Status = null,
    RequestCategory? Category = null,
    RequestPriority? Priority = null,
    string? SearchText = null
    );
}
