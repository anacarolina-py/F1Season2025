using F1Season2025.Competition.Clients;
using F1Season2025.Competition.Data;
using F1Season2025.Competition.Repository;
using F1Season2025.Competition.Repository.Interfaces;
using F1Season2025.Competition.Services;
using F1Season2025.Competition.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<ConnectionDB>();

builder.Services.AddSingleton<ICircuitRepository, CircuitRepository>();
builder.Services.AddSingleton<ICompetitionRepository, CompetitionRepository>();

builder.Services.AddSingleton<ICompetitionService, CompetitionService>();

//builder.Services.AddHttpClient<ITeamServiceClient, TeamServiceClient>(client =>{client.BaseAddress = new Uri("http://team-api");});


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
