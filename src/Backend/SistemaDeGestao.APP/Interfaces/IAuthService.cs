using SistemaDeGestao.APP.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaDeGestao.APP.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
        string GenerateJwtToken(Guid userId, string email, string role);
    }
}
