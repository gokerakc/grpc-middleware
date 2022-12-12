using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Starfish.Core.Models;
using Starfish.Core.Services;
using Starfish.Infrastructure.Data;
using Starfish.Infrastructure.Repositories;
using Starfish.Shared;
using Starfish.Web.HostedServices;
using Starfish.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Starfish injections
builder.Services.AddScoped<IRepository<BankAccount>, BankAccountsRepository>();
builder.Services.AddScoped<IRepository<BankTransaction>, BankTransactionsRepository>();
builder.Services.AddScoped<IBankAccountsService, BankAccountsService>();
builder.Services.AddScoped<IBankTransactionsService, BankTransactionsService>();

// Starfish hosted services
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHostedService<SeedSampleDataService>();    
}

// Starfish middleware injections
builder.Services.AddSingleton<ProcessTimeMiddleware>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(connectionString,
        providerOptions =>
            providerOptions
                .CommandTimeout(60)
                .EnableRetryOnFailure());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ProcessTimeMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();