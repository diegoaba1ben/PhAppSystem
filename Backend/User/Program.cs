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
using Serilog;
using PhAppUser.Application.Services.Validation;

var builder = WebApplication.CreateBuilder(args);

// Cargar las variables de entorno
DotNetEnv.Env.Load();

// Configuración de cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                     ?? Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("La cadena de conexión no está configurada. Verifique el archivo de configuración o las variables de entorno.");
}

// Configuración de base de datos
builder.Services.AddDbContext<PhAppUserDbContext>(options =>
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 4)),
        mysqlOptions => mysqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)
    ));

// Configuración de Kestrel
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5700);
    options.ListenLocalhost(5701, listenOptions => listenOptions.UseHttps());
});

// Configuración de Serilog como logger principal
builder.Host.UseSerilog((context, config) =>
{
    config
        .WriteTo.Console() // Logs en consola
        .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) // Logs diarios
        .Enrich.FromLogContext() // Agrega contexto adicional
        .MinimumLevel.Information(); // Nivel de log mínimo
});

// Configuración de servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar repositorios
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICuentaUsuarioRepository, CuentaUsuarioRepository>();
builder.Services.AddScoped<ISaludRepository, SaludRepository>();
builder.Services.AddScoped<IPensionRepository, PensionRepository>();
builder.Services.AddScoped<IPerfilRepository, PerfilRepository>();
builder.Services.AddScoped<IPermisoRepository, PermisoRepository>();
builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<IRepLegalRepository, RepLegalRepository>();

// Registro de servicios de validación
builder.Services.AddScoped<DatabaseValidationService>();

// Configuración de AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Registro de Queries
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

// Configuración de Swagger
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
