using System.Net.Http.Json;
using BiblioTech2.Web.Client.Models;
using BiblioTech2.Web.Client.Models.Auth;

namespace BiblioTech2.Web.Client.Services.Auth;

/// <summary>
/// Habla con AuthController. Si el login sale bien, le entrega el token al proveedor
/// para que lo guarde y avise a toda la app que ya hay alguien adentro.
/// </summary>
public class AuthApiService(HttpClient http, ProveedorAutenticacion proveedor) : ApiServiceBase(http)
{
    public async Task<RespuestaApi<TokenDto>> LoginAsync(LoginDto dto)
    {
        var respuesta = await EnviarAsync<TokenDto>(() => Http.PostAsJsonAsync("api/auth/login", dto));

        if (respuesta.Exito && respuesta.Datos is not null)
            await proveedor.IniciarSesionAsync(respuesta.Datos);

        return respuesta;
    }

    public Task CerrarSesionAsync() => proveedor.CerrarSesionAsync();
}