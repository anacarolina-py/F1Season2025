using F1Season2025.TeamManagement.Repositories;
using F1Season2025.TeamManagement.Repositories.Teams.Interfaces;
using F1Season2025.TeamManagement.Services;
using F1Season2025.TeamManagement.Services.Teams.Interfaces;
using Infrastructure.TeamManagement.Data.SQL.Connection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<ConnectionDB>();
builder.Services.AddSingleton<ITeamService, TeamService>();
builder.Services.AddSingleton<ITeamRepository, TeamRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
