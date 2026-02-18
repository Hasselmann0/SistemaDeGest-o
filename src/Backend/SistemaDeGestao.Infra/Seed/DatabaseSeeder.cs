using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SistemaDeGestao.Domain.Entities;
using SistemaDeGestao.Domain.Enums;
using SistemaDeGestao.Infra.Data;

namespace SistemaDeGestao.Infra.Seed
{
    public static class DatabaseSeeder
    {
        public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<ApplicationDbContext>>();

            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                var userManager = services.GetRequiredService<UserManager<UserEntity>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                await SeedRolesAsync(roleManager, logger);
                await SeedUsersAsync(userManager, logger);

                logger.LogInformation("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager, ILogger logger)
        {
            foreach (var role in Enum.GetValues<UserRole>())
            {
                var roleName = role.ToString();
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (result.Succeeded)
                    {
                        logger.LogInformation("Role '{RoleName}' created successfully.", roleName);
                    }
                    else
                    {
                        logger.LogError("Failed to create role '{RoleName}': {Errors}", roleName, string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
            }
        }

        private static async Task SeedUsersAsync(UserManager<UserEntity> userManager, ILogger logger)
        {
            if (userManager.Users.Any())
            {
                logger.LogInformation("Users already exist. Skipping user seeding.");
                return;
            }

            var users = new List<(UserEntity User, string Password)>
            {
                (
                    new UserEntity
                    {
                        UserName = "admin@sistema.com",
                        Email = "admin@sistema.com",
                        EmailConfirmed = true,
                        Role = UserRole.User,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    "Admin@123"
                ),
                (
                    new UserEntity
                    {
                        UserName = "user@sistema.com",
                        Email = "user@sistema.com",
                        EmailConfirmed = true,
                        Role = UserRole.User,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    "User@123"
                ),
                (
                    new UserEntity
                    {
                        UserName = "manager@sistema.com",
                        Email = "manager@sistema.com",
                        EmailConfirmed = true,
                        Role = UserRole.Manager,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    "Manager@123"
                )
            };

            foreach (var (user, password) in users)
            {
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, user.Role.ToString());
                    logger.LogInformation("User '{Email}' created and assigned to role '{Role}'.", user.Email, user.Role);
                }
                else
                {
                    logger.LogError("Failed to create user '{Email}': {Errors}", user.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
