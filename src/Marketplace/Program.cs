using Marketplace;
using Marketplace.Api;
using Marketplace.Domain;
using Marketplace.Framework;
using Marketplace.Mongo;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration(b => b.AddJsonFile("appsettings.secrets.json", optional: true, reloadOnChange: true));

var appSettings = builder.Configuration.Get<AppSettings>();

var services = builder.Services;
services.AddSingleton(appSettings);
var mongoClient = new MongoClient(appSettings.Mongo.ConnectionString);
services.AddSingleton<IMongoClient>(mongoClient);
services.AddSingleton(typeof(IMongoRepository<>), typeof(MongoRepository<>));
services.AddSingleton<IApplicationService, ClassifiedAdsApplicationService>();
services.AddSingleton<ICurrencyLookup, InMemoryCurrencyLookup>();
services.AddSingleton<IClassifiedAdRepository, ClassifiedAdRepository>();

services.AddControllers();
services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo
{
    Title = "Marketplace Api",
    Version = "v1"
}));


var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.Run();
