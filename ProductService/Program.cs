using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using DAL.Data;

using System.Globalization;
using BLL.Servicios;
using BLL.Implementaciones;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddSwaggerGen();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = new[] 
    { 
        new CultureInfo("en-US") 
    };
    options.SupportedUICultures = new[] 
    { 
        new CultureInfo("en-US") 
    };

});
builder.Services.AddDbContext<ProductContext>(options => options.
                                              UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddScoped<IProductService, ProductoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IClienteProductoService, ClienteProductoService>();


var app = builder.Build();
app.UseRequestLocalization();
 
using(var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProductContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
