using F1Season2025.RaceControl.Repositories;
using F1Season2025.RaceControl.Repositories.Interfaces;
using F1Season2025.RaceControl.Services;
using F1Season2025.RaceControl.Services.Intefaces;
using Infrastructure.RaceControl.Data.Mongo;
using Infrastructure.RaceControl.Data.Mongo.Settings;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var mongoSettings = builder.Configuration.GetSection("MongoSettings").Get<MongoSettings>();

if (mongoSettings is null)
    throw new Exception("Mongo settings can't be null");

builder.Services.AddSingleton(mongoSettings);

builder.Services.AddSingleton<MongoContext>();

builder.Services.AddScoped<IRaceRepository, RaceRepository>();
builder.Services.AddScoped<IRaceService, RaceService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
