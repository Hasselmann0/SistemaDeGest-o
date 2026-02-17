namespace SistemaDeGestao.APP.DTOs.Auth
{
    public record UserInfoDto(
        Guid Id,
        string Name,
        string Email,
        string Role
    );
}
