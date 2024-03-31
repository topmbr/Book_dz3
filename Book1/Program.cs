using Book1.Interfaces;
using Book1.Services;
using Microsoft.Extensions.DependencyInjection;
using WarehouseApp.Services;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<IDatabaseService, BookServices>(provider => new BookServices(connectionString));

var baseUrl = configuration["HttpRequestSettings:BaseUrl"];
builder.Services.AddHttpClient<IHttpRequestService, HttpRequestService>(client =>
{
    client.BaseAddress = new Uri(baseUrl);
});

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
