using BiblioTech2.Web.Client.Models.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BiblioTech2.Web.Client.Services.Auth;

/// <summary>
/// Le responde a Blazor la pregunta "¿quién está usando la app?".
/// AuthorizeView y todo lo de autorización le preguntan a esta clase.
/// Cuando alguien entra o sale, AVISA (NotifyAuthenticationStateChanged) y Blazor redibuja.
/// </summary>
public class ProveedorAutenticacion(AlmacenSesion almacen) : AuthenticationStateProvider
{
    private static readonly AuthenticationState Anonimo = new(new ClaimsPrincipal(new ClaimsIdentity()));

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var sesion = await almacen.ObtenerAsync();
        if (sesion is null)
            return Anonimo;

        // El token ya venció: lo borramos y seguimos como anónimo.
        if (sesion.Expira <= DateTime.UtcNow)
        {
            await almacen.BorrarAsync();
            return Anonimo;
        }

        return new AuthenticationState(CrearUsuario(sesion));
    }

    public async Task IniciarSesionAsync(TokenDto sesion)
    {
        await almacen.GuardarAsync(sesion);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(CrearUsuario(sesion))));
    }

    public async Task CerrarSesionAsync()
    {
        await almacen.BorrarAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(Anonimo));
    }

    // Un "usuario" en .NET es un ClaimsPrincipal: una identidad con datos (claims) como nombre y rol.
    private static ClaimsPrincipal CrearUsuario(TokenDto sesion)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, sesion.Email),
            new Claim(ClaimTypes.Role, sesion.Rol)
        };

        // El segundo parámetro ("jwt") es lo que hace que IsAuthenticated = true.
        return new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: "jwt"));
    }
}