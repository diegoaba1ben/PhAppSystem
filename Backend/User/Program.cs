using PhAppUser.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using PhAppUser.Infrastructure.Repositories.Interfaces;
using PhAppUser.Infrastructure.Repositories.Implementations;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

//Configuración de Kestrl para escuchar en puertos HTTP y HTTPS
builder.WebHost.ConfigureKestrel(options => 
   {
    options.ListenLocalhost(5246); //Puerto HTTP
    options.ListenLocalhost(7010, ListenOptions => ListenOptions.UseHttps()); // Puerto HTTPS
   });

// Configuración de servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de Logger
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddFile("Logs/app.log");

// Configurar el contexto de la base de datos con MySQL y Pomelo
builder.Services.AddDbContext<PhAppUserDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"), 
        new MySqlServerVersion(new Version(8, 0, 23)) 
    ));
// Registrar repositorios genéricos y específicos
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICuentaUsuarioRepository , CuentaUsuarioRepository>();


// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// Configuración de FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

// Configuración de Swagger en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("AllowAllOrigins");
app.MapControllers();

app.Run();



