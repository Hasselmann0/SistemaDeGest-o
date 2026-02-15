using SistemaDeGestao.Domain.Entities;

namespace SistemaDeGestao.Domain.Interfaces
{
    public interface ILoginRepository
    {
        Task<UserEntity?> GetByEmailAsync(string email);
    }
}
