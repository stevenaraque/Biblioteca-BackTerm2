namespace BiblioTech2.Web.Client.Models.Auth;

public class TokenDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expira { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
}