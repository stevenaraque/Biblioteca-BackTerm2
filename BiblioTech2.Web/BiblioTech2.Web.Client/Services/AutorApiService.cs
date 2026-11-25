using System.Net.Http.Json;
using BiblioTech2.Web.Client.Models;
using BiblioTech2.Web.Client.Models.Autores;

namespace BiblioTech2.Web.Client.Services;


public class AutorApiService(HttpClient http) : ApiServiceBase(http)
{
    private const string Ruta = "api/autores";

    public Task<RespuestaApi<List<AutorDto>>> ListarAsync()
        => EnviarAsync<List<AutorDto>>(() => Http.GetAsync(Ruta));

    public Task<RespuestaApi<AutorDto>> ObtenerAsync(int id)
        => EnviarAsync<AutorDto>(() => Http.GetAsync($"{Ruta}/{id}"));

    public Task<RespuestaApi<AutorDto>> CrearAsync(AutorGuardarDto dto)
        => EnviarAsync<AutorDto>(() => Http.PostAsJsonAsync(Ruta, dto));

    public Task<RespuestaApi<object>> ActualizarAsync(int id, AutorGuardarDto dto)
        => EnviarAsync<object>(() => Http.PutAsJsonAsync($"{Ruta}/{id}", dto));

    public Task<RespuestaApi<object>> EliminarAsync(int id)
        => EnviarAsync<object>(() => Http.DeleteAsync($"{Ruta}/{id}"));
}