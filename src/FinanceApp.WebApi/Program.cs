using FinanceApp.Infrastructure.Data.Extensions;
using FinanceApp.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFinanceAppServices(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.InitialiseDatabaseAsync();
}

app.UseApiServices();

app.Run();