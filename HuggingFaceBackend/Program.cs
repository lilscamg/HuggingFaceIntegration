using HuggingFaceBackend.Helpers;
using HuggingFaceBackend.Models.ConfigSections;
using HuggingFaceBackend.Services;
using HuggingFaceBackend.Services.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
builder.Configuration.AddJsonFile("config.json");
builder.Services.Configure<DatabaseSection>(builder.Configuration.GetSection("Database"));
builder.Services.Configure<HuggingFaceAPISection>(builder.Configuration.GetSection("HuggingFaceAPI"));
builder.Services.AddTransient<IHuggingFaceApiClient, HuggingFaceApiClient>();

// сервисы
builder.Services.AddControllers();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            // указывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = true,
            // строка, представляющая издателя
            ValidIssuer = AuthOptions.ISSUER,
            // будет ли валидироваться потребитель токена
            ValidateAudience = true,
            // установка потребителя токена
            ValidAudience = AuthOptions.AUDIENCE,
            // будет ли валидироваться время существования
            ValidateLifetime = true,
            // установка ключа безопасности
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
        };
    });

var app = builder.Build();

// миддлвейры
app.UseCors(corsBuilder =>
{
    corsBuilder
    .WithOrigins("http://localhost:8080")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials();

});
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

// запуск
app.Run();
