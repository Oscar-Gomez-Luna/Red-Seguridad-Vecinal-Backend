using Backend_RSV.Controllers.Pagos;
using Backend_RSV.Data.Avisos;
using Backend_RSV.Data.Estadisticas;
using Backend_RSV.Data.Mapa;
using Backend_RSV.Data.Usuarios;
using Backend_RSV.Services;
using MiApi.Data;
using Microsoft.EntityFrameworkCore;
using Backend_RSV.Data.Alertas;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System.IO;
using Backend_RSV.Data.Reportes;
using Backend_RSV.Data.Servicios;
using Backend_RSV.Data.Invitados;
using Backend_RSV.Data.Amenidades;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL")));

var logoPath = Path.Combine(AppContext.BaseDirectory, "logo.png");
builder.Services.AddScoped(provider => new ComprobantePdfService(logoPath));
builder.Services.AddScoped<UsuariosData>();
builder.Services.AddScoped<AvisosData>();
builder.Services.AddScoped<PagosData>();
builder.Services.AddScoped<MapaData>();
builder.Services.AddScoped<EstadisticasData>();
builder.Services.AddScoped<AlertaPanicoData>();
builder.Services.AddScoped<FirebaseNotificationService>();
builder.Services.AddScoped<ReporteData>();
builder.Services.AddScoped<ServiciosData>();
builder.Services.AddScoped<IFirebaseDataService, FirebaseDataService>();
builder.Services.AddScoped<InvitadosData>();
builder.Services.AddScoped<QrService>();
builder.Services.AddScoped<AmenidadesData>();
builder.Services.AddSingleton<IAlertasTrackingService, AlertasTrackingService>();
builder.Services.AddHostedService(sp => (AlertasTrackingService)sp.GetRequiredService<IAlertasTrackingService>());

Backend_RSV.Config.FirebaseInitializer.Initialize();

builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", app =>
    {
        app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
var firebasePath = Path.Combine(Directory.GetCurrentDirectory(), "firebase-adminsdk.json");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
    }
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}




app.UseHttpsRedirection();
app.UseCors("NuevaPolitica");
app.UseAuthorization();
app.UseCors("NuevaPolitica");
app.MapControllers();
app.Run();