using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Starfish.Web.Extensions;
using Starfish.Web.Middlewares;
using Starfish.Web.Options;
using Starfish.Web.Watchers;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSerilog(builder.Host)
    .AddApiVersioningDependencies()
    .AddStarfishDependencies()
    .AddFraudCheckerGrpcClient(builder.Configuration)
    .AddStarfishDatabase(builder.Configuration)
    .AddStarfishConfigurationSource(builder.Configuration)
    .AddStarfishHostedServices(builder.Environment.IsDevelopment());

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

// Watchers (Just to try Change token feature)
WatcherHelper.AddGuestListWatcher();

builder.Services.AddGlobalExceptionHandler(builder.Environment.IsDevelopment());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseStarfishRateLimiting();

app.UseProblemDetails();

app.UseMiddleware<PerformanceMonitorMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();