using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaDeGestao.APP.DTOs.Requests
{
    public record ApproveRequestDto(
    string? Comment = null
    );
}
