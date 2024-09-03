using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios
builder.Services.AddControllers();

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Configuración del cliente HTTP para conectar con la API de Flask
builder.Services.AddHttpClient("FlaskApi", client =>
{
    client.BaseAddress = new System.Uri(Environment.GetEnvironmentVariable("FLASK_API_URL") ?? "http://localhost:5000");
});

var app = builder.Build();

// Configuración de la aplicación
app.UseRouting();
app.UseCors("AllowAll");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Configuración de los endpoints
app.MapGet("/pokemons", async (IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient("FlaskApi");
    var response = await client.GetAsync("/pokemons");
    response.EnsureSuccessStatusCode();
    var content = await response.Content.ReadAsStringAsync();
    return Results.Content(content, "application/json");
});

app.MapGet("/recommend", async (IHttpClientFactory httpClientFactory, HttpContext httpContext) =>
{
    var name = httpContext.Request.Query["name"];
    
    if (string.IsNullOrEmpty(name))
    {
        return Results.BadRequest("Pokémon name is required.");
    }

    var client = httpClientFactory.CreateClient("FlaskApi");
    var response = await client.GetAsync($"/recommend?name={name}");
    response.EnsureSuccessStatusCode();
    var content = await response.Content.ReadAsStringAsync();
    
    return Results.Content(content, "application/json");
});

app.Run("http://0.0.0.0:3000");