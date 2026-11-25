using BiblioTech2.Web.Client.Services;
using BiblioTech2.Web.Client.Services.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

// Este Program.cs se ejecuta DENTRO DEL NAVEGADOR (WebAssembly), no en el servidor.
var builder = WebAssemblyHostBuilder.CreateDefault(args);

// La dirección de la API se lee de wwwroot/appsettings.json → "ApiUrl".
var apiUrl = builder.Configuration["ApiUrl"]
    ?? throw new InvalidOperationException("Falta 'ApiUrl' en wwwroot/appsettings.json");

// ---------- 1. SESIÓN Y AUTORIZACIÓN ----------
builder.Services.AddAuthorizationCore();                // habilita AuthorizeView
builder.Services.AddCascadingAuthenticationState();     // reparte "quién soy" a todos los componentes
builder.Services.AddScoped<AlmacenSesion>();            // guarda el token en localStorage
builder.Services.AddScoped<ProveedorAutenticacion>();   // responde "¿quién está conectado?"
builder.Services.AddScoped<AuthenticationStateProvider>(
    sp => sp.GetRequiredService<ProveedorAutenticacion>());   // Blazor le pregunta a NUESTRO proveedor

// ---------- 2. HTTP: un HttpClient con el "mensajero" que pega el token ----------
builder.Services.AddScoped(sp =>
{
    var manejador = new ManejadorToken(
        sp.GetRequiredService<AlmacenSesion>(),
        sp.GetRequiredService<ProveedorAutenticacion>())
    {
        InnerHandler = new HttpClientHandler()   // el que de verdad envía la petición
    };

    return new HttpClient(manejador) { BaseAddress = new Uri(apiUrl) };
});


builder.Services.AddScoped<AuthApiService>();
builder.Services.AddScoped<AutorApiService>();

// Arranca el host WebAssembly: sin esto los componentes interactivos nunca se hidratan.
await builder.Build().RunAsync();