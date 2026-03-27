using Ability.Api.Interfaces;
using Ability.Api.Services;
using Ability.Domain.Interfaces;
using Ability.Infrastructure.Repositories;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("Mongo");

var dbName = builder.Configuration["DatabaseName"] ?? "AbilityDb";

builder.Services.AddSingleton<IMongoClient>(new MongoClient(connectionString));

builder.Services.AddScoped<INoticiaRepository>(sp =>
    new NoticiaRepository(sp.GetRequiredService<IMongoClient>(), dbName));

builder.Services.AddScoped<INoticiaService, NoticiaService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
