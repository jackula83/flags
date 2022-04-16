using FlagSense.FlagService.Api.Options;
using FlagSense.FlagService.Domain.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

Debugger.Break();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddEnvironmentVariables();
var databaseSection = builder.Configuration.GetSection(DatabaseOptions.OptionName);

// Configure database context
var connectionString = $@"
    Server={databaseSection[nameof(DatabaseOptions.Server)]},{databaseSection[nameof(DatabaseOptions.Port)]};
    Database={databaseSection[nameof(DatabaseOptions.Name)]};
    User Id={databaseSection[nameof(DatabaseOptions.Username)]};
    Password={databaseSection[nameof(DatabaseOptions.Password)]};";
builder.Services.AddDbContext<FsDbContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Scoped);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
