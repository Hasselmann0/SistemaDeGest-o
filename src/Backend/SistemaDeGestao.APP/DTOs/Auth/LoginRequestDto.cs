using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SistemaDeGestao.APP.DTOs.Auth
{
    public record LoginRequestDto(
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    string Email,

    [Required(ErrorMessage = "Senha é obrigatória")]
    string Password
    );
}
