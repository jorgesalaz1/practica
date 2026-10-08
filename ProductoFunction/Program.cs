using BLL.Implementaciones;
using BLL.Servicios;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductoFunction;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();
builder.Services.AddScoped< IBlobService, BlobService>();
builder.Services.AddScoped<Function1>();

await builder.Build().RunAsync();