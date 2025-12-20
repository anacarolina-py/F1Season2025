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
builder.Services.AddSingleton<IBossService, BossService>();
builder.Services.AddSingleton<IBossRepository, BossRepository>();
builder.Services.AddSingleton<IDriverService, DriverService>();
builder.Services.AddSingleton<IDriverRepository, DriverRepository>();
builder.Services.AddSingleton<IAerodynamicEngineerService, AerodynamicEngineerService>();
builder.Services.AddSingleton<IAerodynamicEngineerRepository, AerodynamicEngineerRepository>();
builder.Services.AddSingleton<IPowerEngineerService, PowerEngineerService>();
builder.Services.AddSingleton<IPowerEngineerRepository, PowerEngineerRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
