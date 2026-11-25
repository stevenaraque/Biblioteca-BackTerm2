namespace BiblioTech2.Web.Client.Models;

public class RespuestaApi<T>
{
    public bool Exito { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public T? Datos { get; set; }
    public Dictionary<string, string[]>? Errores { get; set; }

    /// <summary>Respuesta fabricada en el front cuando la API no contestó (apagada, sin red, CORS...).</summary>
    public static RespuestaApi<T> Fallo(string mensaje) => new() { Exito = false, Mensaje = mensaje };
}