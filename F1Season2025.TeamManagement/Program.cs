using F1Season2025.TeamManagement.Repositories.Cars;
using F1Season2025.TeamManagement.Repositories.Cars.Interfaces;
using F1Season2025.TeamManagement.Repositories.Staffs.Bosses;
using F1Season2025.TeamManagement.Repositories.Staffs.Bosses.Interfaces;
using F1Season2025.TeamManagement.Repositories.Staffs.Drivers;
using F1Season2025.TeamManagement.Repositories.Staffs.Drivers.Interfaces;
using F1Season2025.TeamManagement.Repositories.Staffs.Engineers.AerodynamicEngineers;
using F1Season2025.TeamManagement.Repositories.Staffs.Engineers.AerodynamicEngineers.Interfaces;
using F1Season2025.TeamManagement.Repositories.Staffs.Engineers.PowerEngineers;
using F1Season2025.TeamManagement.Repositories.Staffs.Engineers.PowerEngineers.Interfaces;
using F1Season2025.TeamManagement.Repositories.Teams;
using F1Season2025.TeamManagement.Repositories.Teams.Interfaces;
using F1Season2025.TeamManagement.Services.Cars;
using F1Season2025.TeamManagement.Services.Cars.Interfaces;
using F1Season2025.TeamManagement.Services.Staffs.Bosses;
using F1Season2025.TeamManagement.Services.Staffs.Bosses.Interfaces;
using F1Season2025.TeamManagement.Services.Staffs.Drivers;
using F1Season2025.TeamManagement.Services.Staffs.Drivers.Interfaces;
using F1Season2025.TeamManagement.Services.Staffs.Engineers.AerodynamicEngineers;
using F1Season2025.TeamManagement.Services.Staffs.Engineers.AerodynamicEngineers.Interfaces;
using F1Season2025.TeamManagement.Services.Staffs.Engineers.PowerEngineers;
using F1Season2025.TeamManagement.Services.Staffs.Engineers.PowerEngineers.Interfaces;
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
builder.Services.AddScoped<IBossService, BossService>();
builder.Services.AddScoped<IBossRepository, BossRepository>();
builder.Services.AddScoped<IDriverService, DriverService>();
builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<IAerodynamicEngineerService, AerodynamicEngineerService>();
builder.Services.AddScoped<IAerodynamicEngineerRepository, AerodynamicEngineerRepository>();
builder.Services.AddScoped<IPowerEngineerService, PowerEngineerService>();
builder.Services.AddScoped<IPowerEngineerRepository, PowerEngineerRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
