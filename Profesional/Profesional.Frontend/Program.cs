using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Profesional.Frontend;
using Profesional.Frontend.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configurar HttpClient con la URL de la API (definida en wwwroot/appsettings.json)
var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7211/";

builder.Services.AddScoped<AuthTokenHandler>();

builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("API"));

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PacienteService>();

await builder.Build().RunAsync();