using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SistemaDeGestao.APP.DTOs.Requests
{
    public record RejectRequestDto(
    [Required(ErrorMessage = "Justificativa é obrigatória para rejeição")]
    [StringLength(1000, MinimumLength = 10, 
            ErrorMessage = "Justificativa deve ter entre 10 e 1000 caracteres")]
    string Comment = ""
    );
}
