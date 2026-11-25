using System.Net.Http.Json;
using System.Text.Json;
using BiblioTech2.Web.Client.Models;

namespace BiblioTech2.Web.Client.Services;

public abstract class ApiServiceBase(HttpClient http)
{
    protected HttpClient Http { get; } = http;

    protected async Task<RespuestaApi<T>> EnviarAsync<T>(Func<Task<HttpResponseMessage>> peticion)
    {
        try
        {
            using var respuesta = await peticion();

            // Aunque el código sea 400/401/404/409/500, nuestra API manda el sobre en el cuerpo.
            var cuerpo = await respuesta.Content.ReadFromJsonAsync<RespuestaApi<T>>();
            return cuerpo ?? RespuestaApi<T>.Fallo($"La API respondió {(int)respuesta.StatusCode} sin contenido.");
        }
        catch (HttpRequestException)
        {
            return RespuestaApi<T>.Fallo("No fue posible conectar con la API. ¿Está en ejecución?");
        }
        catch (JsonException)
        {
            return RespuestaApi<T>.Fallo("La API devolvió una respuesta con un formato inesperado.");
        }
    }
}