using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaDeGestao.APP.DTOs.Requests;
using SistemaDeGestao.APP.Interfaces;
using SistemaDeGestao.Domain.Enums;
using System.Security.Claims;

namespace SistemaDeGestao.API.Controllers
{
    [Route("api/requests")]
    [ApiController]
    [Authorize]
    public class RequestController : ControllerBase
    {
        private readonly IResquestService _requestService;

        public RequestController(IResquestService requestService)
        {
            _requestService = requestService;
        }

        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst("userId")?.Value
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdClaim!);
        }

        private bool IsManager()
        {
            return User.IsInRole("Manager");
        }

    
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] RequestStatus? status,
            [FromQuery] RequestCategory? category,
            [FromQuery] RequestPriority? priority,
            [FromQuery] string? searchText)
        {
            var filter = new RequestFilterDto(status, category, priority, searchText);
            var userId = GetUserId();
            var isManager = IsManager();

            var requests = await _requestService.GetAllAsync(filter, userId, isManager);
            return Ok(requests);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var userId = GetUserId();
            var isManager = IsManager();

            var request = await _requestService.GetByIdAsync(id, userId, isManager);
            if (request is null)
                return NotFound(new { Message = "Solicitação não encontrada ou acesso negado" });

            return Ok(request);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRequestDto dto)
        {
            var userId = GetUserId();
            var request = await _requestService.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = request.Id }, request);
        }

        [HttpPost("{id:guid}/approve")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Approve(Guid id, [FromBody] ApproveRequestDto dto)
        {
            var managerId = GetUserId();
            var result = await _requestService.ApproveAsync(id, dto, managerId);

            if (result is null)
                return BadRequest(new { Message = "Não foi possível aprovar. Solicitação não encontrada ou não está pendente." });

            return Ok(result);
        }

        [HttpPost("{id:guid}/reject")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Reject(Guid id, [FromBody] RejectRequestDto dto)
        {
            var managerId = GetUserId();
            var result = await _requestService.RejectAsync(id, dto, managerId);

            if (result is null)
                return BadRequest(new { Message = "Não foi possível rejeitar. Solicitação não encontrada ou não está pendente." });

            return Ok(result);
        }


        [HttpGet("{id:guid}/history")]
        public async Task<IActionResult> GetHistory(Guid id)
        {
            var userId = GetUserId();
            var isManager = IsManager();

            var request = await _requestService.GetByIdAsync(id, userId, isManager);
            if (request is null)
                return NotFound(new { Message = "Solicitação não encontrada ou acesso negado" });

            return Ok(request.StatusHistory);
        }
    }
}