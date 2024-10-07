using FinanceApp.WebApi;
using FinanceApp.Application;
using FinanceApp.Application.Data;
using FinanceApp.Domain.Models;
using FinanceApp.Infrastructure;
using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Data.Extensions;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Registrar IApplicationDbContext y su implementación
builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseApiServices();

if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}

app.Run();