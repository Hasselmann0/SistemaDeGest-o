using SistemaDeGestao.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SistemaDeGestao.APP.DTOs.Requests
{
    public record CreateRequestDto(
    [Required(ErrorMessage = "Título é obrigatório")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Título deve ter entre 3 e 200 caracteres")]
    string Title,

    [Required(ErrorMessage = "Descrição é obrigatória")]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "Descrição deve ter entre 10 e 2000 caracteres")]
    string Description,

    [Required(ErrorMessage = "Categoria é obrigatória")]
    RequestCategory Category,

    [Required(ErrorMessage = "Prioridade é obrigatória")]
    RequestPriority Priority
    );
}
