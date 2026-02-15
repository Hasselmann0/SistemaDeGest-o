using Microsoft.EntityFrameworkCore;
using SistemaDeGestao.Domain.Entities;
using SistemaDeGestao.Domain.Interfaces;
using SistemaDeGestao.Infra.Data;

namespace SistemaDeGestao.Infra.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly ApplicationDbContext _context;

        public LoginRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserEntity?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
        }
    }
}
