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

// �������
builder.Services.AddControllers();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            // ���������, ����� �� �������������� �������� ��� ��������� ������
            ValidateIssuer = true,
            // ������, �������������� ��������
            ValidIssuer = AuthOptions.ISSUER,
            // ����� �� �������������� ����������� ������
            ValidateAudience = true,
            // ��������� ����������� ������
            ValidAudience = AuthOptions.AUDIENCE,
            // ����� �� �������������� ����� �������������
            ValidateLifetime = true,
            // ��������� ����� ������������
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            // ��������� ����� ������������
            ValidateIssuerSigningKey = true,
        };
    });

var app = builder.Build();

// ����������
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

// ������
app.Run();
