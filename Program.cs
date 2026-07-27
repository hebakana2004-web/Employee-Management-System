using BackendTest.Context;
using BackendTest.Interfaces;
using BackendTest.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Text;
using Oracle.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/// Serilog. Writinng to file
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.Console()
    .WriteTo.File(
        "Logs/log-.txt",
        rollingInterval: RollingInterval.Day
    )
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("MySuperSecretKeyForJwtToken12345"))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<ILoginService, LoginService>();



builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

// Dependency Injection
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddDbContext<EmployeeDBContext>(options =>
    options.UseOracle(
        builder.Configuration.GetConnectionString("OracleConnection")));
// Registering the DbContext (EmployeeDBContext) in the Dependency Injection container
// This allows ASP.NET Core to create and manage the DbContext automatically
// Telling Entity Framework Core to use SQL Server as the database provider options.UseSqlServer(
// Getting the connection string named "EmployeesConnection" from appsettings.json builder.Configuration.GetConnectionString("EmployeesConnection")



builder.Services.AddDbContext<UsersDBContext>(options =>
    options.UseOracle(
        builder.Configuration.GetConnectionString("OracleConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting(); 

app.UseCors("AllowAngular"); 

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();