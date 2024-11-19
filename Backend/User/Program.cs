using PhAppUser.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using PhAppUser.Infrastructure.Repositories.Interfaces;
using PhAppUser.Infrastructure.Repositories.Implementations;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using DotNetEnv;
using AutoMapper;
using PhAppUser.Application.Mappers;
using PhAppUser.Application.Queries;
using PhAppUser.Application.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Cargar las variables de entorno usando DotNetEnv
DotNetEnv.Env.Load();

// Configuración de cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                     ?? Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("La cadena de conexión no está configurada correctamente.");
}

builder.Services.AddDbContext<PhAppUserDbContext>(options =>
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 4))
    ));

// Configuración de Kestrel para puertos HTTP y HTTPS
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5600);
    // options.ListenLocalhost(7010, listenOptions => listenOptions.UseHttps());
});

// Configuración de servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de Logger
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddFile("Logs/app.log");

// Registrar repositorios genéricos y específicos
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICuentaUsuarioRepository, CuentaUsuarioRepository>();

// Registro de AutoMapper
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<CuentaUsuarioMappingProfile>();
    config.AddProfile<RepLegalMappingProfile>();
    config.AddProfile<SaludMappingProfile>();
    config.AddProfile<PensionMappinProfile>();
    config.AddProfile<PermisoMappingProfile>();
    config.AddProfile<RolMappingProfile>();
    config.AddProfile<AreaMappingProfile>();
    config.AddProfile<AdvancedUserMappingProfile>();
});

// Registro de Queries (ajustado el error de sintaxis y nombre)
builder.Services.AddScoped<CuentaUsuarioQueries>();
builder.Services.AddScoped<AdvancedQuery>();

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
