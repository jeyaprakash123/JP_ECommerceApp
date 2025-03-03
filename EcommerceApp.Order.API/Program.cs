using System.Text;
using Confluent.Kafka;
using EcommerceApp.Order.Api.Application.Interface;
using EcommerceApp.Order.Api.Application.Service;
using EcommerceApp.Order.Api.Domain.Interface;
using EcommerceApp.Order.Api.Infrastructure.DataContext;
using EcommerceApp.Order.Api.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Aspire;

var builder = WebApplication.CreateBuilder(args);

builder.AddSqlServerDbContext<OrderDbContext>("ECommerceOrder");
builder.AddServiceDefaults();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.

builder.Services.AddControllers();


// SwaggerCode Config

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order_Service_Api", Version = "v1" });

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

//Register service in IOC Container

builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();

builder.Services.AddSingleton<IConsumer<string, string>>(sp =>
{
    var config = new ConsumerConfig
    {
        BootstrapServers = "localhost:9092",
        GroupId = "order-service-group",
        AutoOffsetReset = AutoOffsetReset.Earliest
    };
    return new ConsumerBuilder<string, string>(config).Build();
});

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
await OrderDbContext.EnsureDbCreatedAsync(app.Services, initialImportDataDir);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecom_Order_Api");
    });
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
