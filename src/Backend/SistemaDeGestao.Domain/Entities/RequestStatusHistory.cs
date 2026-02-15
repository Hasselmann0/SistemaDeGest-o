using SistemaDeGestao.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaDeGestao.Domain.Entities
{
    public class RequestStatusHistory : BaseEntity
    {
        public Guid RequestId { get; set; }
        public RequestStatus FromStatus { get; set; }
        public RequestStatus ToStatus { get; set; }
        public string ChangedByUserId { get; set; } = string.Empty;
        public string? Comment { get; set; }

        // Navigation Properties
        public RequestEntity Request { get; set; } = null!;
        public UserEntity ChangedByUser { get; set; } = null!;
    }
}
