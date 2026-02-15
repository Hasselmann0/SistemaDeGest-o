using Microsoft.AspNetCore.Identity;
using SistemaDeGestao.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaDeGestao.Domain.Entities
{
    public class UserEntity : IdentityUser
    {
        public UserRole Role { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public ICollection<RequestEntity> CreatedRequests { get; set; } = [];
        public ICollection<RequestStatusHistory> StatusHistories { get; set; } = [];
    }
}
