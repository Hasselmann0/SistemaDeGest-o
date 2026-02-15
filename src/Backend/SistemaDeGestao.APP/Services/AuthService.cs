using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SistemaDeGestao.APP.DTOs.Auth;
using SistemaDeGestao.APP.Interfaces;
using SistemaDeGestao.Domain.Entities;
using SistemaDeGestao.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SistemaDeGestao.APP.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILoginRepository _loginRepository;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(
            ILoginRepository loginRepository,
            UserManager<UserEntity> userManager,
            IConfiguration configuration)
        {
            _loginRepository = loginRepository;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
        {
            var user = await _loginRepository.GetByEmailAsync(request.Email);
            if (user is null)
                return null;

            var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isValidPassword)
                return null;

            var token = GenerateJwtToken(Guid.Parse(user.Id), user.Email!, user.Role.ToString());
            var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationInMinutes"] ?? "60");

            return new LoginResponseDto(
                token,
                DateTime.UtcNow.AddMinutes(expirationMinutes),
                new UserInfoDto(
                    Guid.Parse(user.Id),
                    user.UserName ?? user.Email!,
                    user.Email!,
                    user.Role.ToString()
                )
            );
        }

        public string GenerateJwtToken(Guid userId, string email, string role)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var secretKey = jwtSettings["Secret"]!;
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expirationMinutes = int.Parse(jwtSettings["ExpirationInMinutes"] ?? "60");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim("userId", userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
