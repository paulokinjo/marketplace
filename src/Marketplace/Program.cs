using Marketplace.Api;
using Marketplace.Domain;
using Marketplace.Framework;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddSingleton<IApplicationService, ClassifiedAdsApplicationService>();
services.AddSingleton<IClassifiedAdRepository>();

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
