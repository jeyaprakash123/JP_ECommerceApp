using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ECommerceApp.Product.Api.Infrastructure.DataContext;
using ECommerceApp.Product.Api.Application.Interface;
using ECommerceApp.Product.Api.Application.Services;
using ECommerceApp.Product.Api.Domain.Interface;
using ECommerceApp.Product.Api.Infrastructure.Repository;
using Confluent.Kafka;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.AddSqlServerDbContext<ProductDbContext>("ECommerceProduct");

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product_Service_Api", Version = "v1" });

    // Add JWT Authentication support
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter your token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
});
builder.Services.AddOpenApi();

builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IProductRepository,ProductRepository>();


builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
});


// Register Kafka producer
builder.Services.AddSingleton<IProducer<string, string>>(sp =>
{
    var config = new ProducerConfig
    {
        BootstrapServers = "localhost:9092/"
    };
    return new ProducerBuilder<string, string>(config).Build();
});


var app = builder.Build();

var initialImportDataDir = builder.Configuration["ImportInitialDataDir"];
await ProductDbContext.EnsureDbCreatedAsync(app.Services, initialImportDataDir);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecom_Product_Api");
    });
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
