using F1Season2025.TeamManagement.Repositories.Cars;
using F1Season2025.TeamManagement.Repositories.Cars.Interfaces;
using F1Season2025.TeamManagement.Repositories.Teams;
using F1Season2025.TeamManagement.Repositories.Teams.Interfaces;
using F1Season2025.TeamManagement.Services.Cars;
using F1Season2025.TeamManagement.Services.Cars.Interfaces;
using F1Season2025.TeamManagement.Services.Teams;
using F1Season2025.TeamManagement.Services.Teams.Interfaces;
using Infrastructure.TeamManagement.Data.SQL.Connection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<ConnectionDB>();
builder.Services.AddSingleton<ITeamService, TeamService>();
builder.Services.AddSingleton<ITeamRepository, TeamRepository>();
builder.Services.AddSingleton<ICarService, CarService>();
builder.Services.AddSingleton<ICarRepository, CarRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
