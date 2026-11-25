using BiblioTech2.Web.Client.Models.Auth;
using Microsoft.JSInterop;
using System.Text.Json;

namespace BiblioTech2.Web.Client.Services.Auth;

/// <summary>
/// Guarda la sesión (token + email + rol + vencimiento) en el localStorage del navegador.
/// localStorage sobrevive a recargar la página y a cerrar la pestaña.
/// Se usa JavaScript del navegador desde C# con IJSRuntime (JS interop).
/// </summary>
public class AlmacenSesion(IJSRuntime js)
{
    private const string Clave = "bibliotech.sesion";

    public async Task GuardarAsync(TokenDto sesion)
        => await js.InvokeVoidAsync("localStorage.setItem", Clave, JsonSerializer.Serialize(sesion));

    public async Task<TokenDto?> ObtenerAsync()
    {
        var json = await js.InvokeAsync<string?>("localStorage.getItem", Clave);
        if (string.IsNullOrWhiteSpace(json))
            return null;

        try
        {
            return JsonSerializer.Deserialize<TokenDto>(json);
        }
        catch (JsonException)
        {
            return null;   // alguien dañó el valor a mano: lo tratamos como "sin sesión"
        }
    }

    public async Task BorrarAsync()
        => await js.InvokeVoidAsync("localStorage.removeItem", Clave);
}