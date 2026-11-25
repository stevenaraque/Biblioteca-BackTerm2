using System.Net;
using System.Net.Http.Headers;

namespace BiblioTech2.Web.Client.Services.Auth;

/// <summary>
/// "Mensajero" que se mete entre el HttpClient y la red.
/// ANTES de cada petición: si hay sesión, pega el encabezado Authorization: Bearer {token}.
/// DESPUÉS: si la API responde 401 (token vencido o inválido), cierra la sesión del front.
/// Así ningún servicio ni página tiene que acordarse del token.
/// </summary>
public class ManejadorToken(AlmacenSesion almacen, ProveedorAutenticacion proveedor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage peticion, CancellationToken cancelacion)
    {
        var sesion = await almacen.ObtenerAsync();
        if (sesion is not null)
            peticion.Headers.Authorization = new AuthenticationHeaderValue("Bearer", sesion.Token);

        var respuesta = await base.SendAsync(peticion, cancelacion);

        // Mandamos token y la API lo rechazó → ese token ya no sirve.
        if (respuesta.StatusCode == HttpStatusCode.Unauthorized && sesion is not null)
            await proveedor.CerrarSesionAsync();

        return respuesta;
    }
}