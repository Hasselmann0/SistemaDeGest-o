using SistemaDeGestao.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaDeGestao.Domain.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
