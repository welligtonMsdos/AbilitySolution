using Ability.Domain.Interfaces;
using Ability.Infrastructure.Repositories;
using Ability.Worker.Work;
using MongoDB.Driver;

var builder = Host.CreateApplicationBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Mongo");

var dbName = builder.Configuration["DatabaseName"] ?? "AbilityDb";

builder.Services.AddSingleton<IMongoClient>(new MongoClient(connectionString));

builder.Services.AddScoped<INoticiaRepository>(sp =>
    new NoticiaRepository(sp.GetRequiredService<IMongoClient>(), dbName));

builder.Services.AddHostedService<RpaWorkService>();

var host = builder.Build();

host.Run();
