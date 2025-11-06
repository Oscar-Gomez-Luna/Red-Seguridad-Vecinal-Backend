using MiApi.Data;
using Microsoft.EntityFrameworkCore;
using Backend_RSV.Data.Alertas;
using Backend_RSV.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// 🔥 AGREGAR IGNORECYCLES PARA EVITAR CICLOS JSON:
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// Database Context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL")));

// 🔥 CONFIGURAR FIREBASE
var firebasePath = Path.Combine(Directory.GetCurrentDirectory(), "Firebase-Credentials", "firebase-adminsdk.json");
if (File.Exists(firebasePath))
{
    FirebaseApp.Create(new AppOptions()
    {
        Credential = GoogleCredential.FromFile(firebasePath),
    });
    Console.WriteLine("✅ Firebase configurado correctamente");
}
else
{
    Console.WriteLine($"⚠️  Archivo firebase-adminsdk.json no encontrado en: {firebasePath}");
    Console.WriteLine("⚠️  Las notificaciones funcionarán en modo simulación");
}

// 🔥 REGISTRAR TUS SERVICIOS PERSONALIZADOS:
builder.Services.AddScoped<AlertaPanicoData>();
builder.Services.AddScoped<FirebaseNotificationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
app.UseAuthorization();
app.MapControllers();

app.Run();