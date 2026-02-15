using Microsoft.AspNetCore.Mvc;
using SistemaDeGestao.APP.DTOs.Auth;
using SistemaDeGestao.APP.Interfaces;

namespace SistemaDeGestao.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);

            if (result is null)
                return Unauthorized(new { Message = "Email ou senha inválidos" });

            return Ok(result);
        }
    }
}
