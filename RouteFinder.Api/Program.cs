using Microsoft.OpenApi.Models;
using RouteFinder.Application;
using RouteFinder.Cache.Redis;
using RouteFinder.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Route Finder",
            Version = "v1"
        }
    );

    var filePath = Path.Combine(System.AppContext.BaseDirectory, "RouteFinder.Api.xml");
    c.IncludeXmlComments(filePath);
});

builder.Services.AddMemoryCache();
builder.Services.AddSearchProviders();
builder.Services.AddSearchProvidersImplementations();
builder.Services.AddRouteCache();
builder.Services.AddRouteCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization(); 

app.MapControllers();

app.Run();
