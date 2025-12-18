using F1Season2025.Engineering.Repositories;
using F1Season2025.Engineering.Services;
using Infrastructure.Engeneering.Data.Client;
using Infrastructure.Engeneering.Data.SQL;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<SqlConnectionFactory>();
builder.Services.AddScoped(sp =>
    new SqlConnectionFactory(
        builder.Configuration.GetConnectionString("SqlServer")!
    ));

builder.Services.AddScoped<EngineeringService>();
builder.Services.AddScoped<EngineeringRepository>();


builder.Services.AddHttpClient<TeamManagementClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost5001/team/api/");
});




var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
