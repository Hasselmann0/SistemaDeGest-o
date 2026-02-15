using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaDeGestao.APP.DTOs.Auth
{
    public record LoginResponseDto(
    string Token,
    DateTime ExpiresAt,
    UserInfoDto User
    );

    public record UserInfoDto(
        Guid Id,
        string Name,
        string Email,
        string Role
    );
}
