using Microsoft.EntityFrameworkCore;
using SistemaDeGestao.Domain.Entities;
using SistemaDeGestao.Domain.Enums;
using SistemaDeGestao.Domain.Interfaces;
using SistemaDeGestao.Infra.Data;

namespace SistemaDeGestao.Infra.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public RequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RequestEntity?> GetByIdAsync(Guid id)
        {
            return await _context.Requests
                .Include(r => r.CreatedByUser)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<RequestEntity?> GetByIdWithHistoryAsync(Guid id)
        {
            return await _context.Requests
                .Include(r => r.CreatedByUser)
                .Include(r => r.StatusHistories)
                    .ThenInclude(h => h.ChangedByUser)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<RequestEntity>> GetAllAsync(
            RequestStatus? status = null,
            RequestCategory? category = null,
            RequestPriority? priority = null,
            string? searchText = null,
            string? userId = null)
        {
            var query = _context.Requests
                .Include(r => r.CreatedByUser)
                .AsQueryable();

            if (status.HasValue)
                query = query.Where(r => r.Status == status.Value);

            if (category.HasValue)
                query = query.Where(r => r.Category == category.Value);

            if (priority.HasValue)
                query = query.Where(r => r.Priority == priority.Value);

            if (!string.IsNullOrWhiteSpace(searchText))
                query = query.Where(r =>
                    r.Title.Contains(searchText) ||
                    r.Description.Contains(searchText));

            if (!string.IsNullOrWhiteSpace(userId))
                query = query.Where(r => r.CreatedByUserId == userId);

            return await query
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<RequestEntity> CreateAsync(RequestEntity request)
        {
            request.Id = Guid.NewGuid();
            request.CreatedAt = DateTime.UtcNow;

            await _context.Requests.AddAsync(request);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(request.Id) ?? request;
        }

        public async Task<RequestEntity> UpdateAsync(RequestEntity request)
        {
            request.UpdatedAt = DateTime.UtcNow;
            _context.Requests.Update(request);
            await _context.SaveChangesAsync();

            return request;
        }

        public async Task AddStatusHistoryAsync(RequestStatusHistory history)
        {
            history.Id = Guid.NewGuid();
            history.CreatedAt = DateTime.UtcNow;

            await _context.RequestStatusHistories.AddAsync(history);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<RequestStatusHistory>> GetHistoryByRequestIdAsync(Guid requestId)
        {
            return await _context.RequestStatusHistories
                .Include(h => h.ChangedByUser)
                .Where(h => h.RequestId == requestId)
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync();
        }
    }
}
