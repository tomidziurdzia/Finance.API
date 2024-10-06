using FinanceApp.WebApi;
using FinanceApp.Application;
using FinanceApp.Application.Data;
using FinanceApp.Application.Features.Users.Queries.GetUsers;
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

builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

var app = builder.Build();

app.UseApiServices();

if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}

app.Run();