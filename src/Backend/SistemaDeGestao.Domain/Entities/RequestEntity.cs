using SistemaDeGestao.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaDeGestao.Domain.Entities
{
    public class RequestEntity : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public RequestCategory Category { get; set; }
        public RequestPriority Priority { get; set; }
        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        // Foreign Key
        public string CreatedByUserId { get; set; } = string.Empty;

        // Navigation Properties
        public UserEntity CreatedByUser { get; set; } = null!;
        public ICollection<RequestStatusHistory> StatusHistories { get; set; } = [];
    }
}
