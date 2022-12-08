using Microsoft.OpenApi.Models;
using StorageService;
using StorageService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options => options.AddPolicy("AllowCors",
                           builder =>
                           {
                               builder
                                .AllowAnyOrigin()
                                .WithMethods("GET", "PUT", "POST", "DELETE")
                                .AllowAnyHeader();
                           }));

ConnectionStrings.AzureStorageConnectionString = builder.Configuration.GetSection("AzureConnectionStrings")["CloudConStr"];

builder.Services.AddSingleton<IBlobStorage, BlobStorage>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "StorageService.API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StorageService.API v1"));
}
app.UseCors("AllowCors");

app.UseAuthorization();

app.MapControllers();

app.Run();
