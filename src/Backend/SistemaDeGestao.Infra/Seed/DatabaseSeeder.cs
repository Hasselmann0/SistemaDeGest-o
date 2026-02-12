using SistemaDeGestao.Domain.Entities;
using SistemaDeGestao.Domain.Enums;
using SistemaDeGestao.Infra.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaDeGestao.Infra.Seed
{
    public class DatabaseSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                var users = new List<UserEntity>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Administrador",
                    Email = "admin@sistema.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    Role = UserRole.Manager,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Usuário Solicitante",
                    Email = "user@sistema.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),
                    Role = UserRole.User,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Gerente Aprovador",
                    Email = "manager@sistema.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Manager@123"),
                    Role = UserRole.Manager,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();
            }
        }

    }
}
