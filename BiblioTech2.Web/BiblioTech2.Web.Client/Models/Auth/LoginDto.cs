using System.ComponentModel.DataAnnotations;

namespace BiblioTech2.Web.Client.Models.Auth;

/// <summary>Lo que el front ENVÍA a POST api/auth/login.</summary>
public class LoginDto
{
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public string Password { get; set; } = string.Empty;
}