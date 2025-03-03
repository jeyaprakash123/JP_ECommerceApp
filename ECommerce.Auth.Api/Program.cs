using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerceApp.Auth.Api.Infrastructure.DataAccess;
using ECommerceApp.Auth.Api.Domain.Models;
using ECommerceApp.Auth.Api.Service;
using ECommerceApp.Auth.Api;
using System;
using ECommerceApp.Auth.Api.Domain.Interface;
using ECommerceApp.Auth.Api.Infrastructure.Repository;
using System.Reflection.Metadata.Ecma335;
using ECommerceApp.Auth.Api.Domain.Model;
using System.Data;
using Microsoft.Extensions.Configuration.EnvironmentVariables;


var builder = WebApplication.CreateBuilder(args);

builder.AddSqlServerDbContext<DataContext>("ECommerceAuth");



builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();

var initialImportDataDir = builder.Configuration["ImportInitialDataDir"];
await DataContext.EnsureDbCreatedAsync(app.Services, initialImportDataDir);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecom_API");
    });
    app.MapOpenApi();
}
app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapPost("/login", async ([FromBody] LoginRequest loginRequest, IAuthService authService, IConfiguration config) =>
{
    var (username,password) = (loginRequest.UserName,loginRequest.Password);
    var user = await authService.FindByNameAsync(username);
    if (user is not null)
    {
        if (BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.Role,user.Roles.RoleName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = new JwtSecurityToken(
                    expires: DateTime.UtcNow.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"])),
                        SecurityAlgorithms.HmacSha256)
                    );

            return Results.Ok(new { Token = tokenHandler.WriteToken(token) });

        }
    }
    return Results.Unauthorized();
});

app.MapPost("/register", async ([FromBody] Login login, IAuthService authService) =>
{
   // var (username, password) = (createUser.UserName, createUser.Password);
    var result = await authService.CreateUserAsync(login);

    if (result is null)
    {
        return Results.BadRequest();
    }

    return Results.Ok("Successfully Registered");
});


app.MapPatch("/UpdateUserPassword", async (string userName,string password,IAuthService authService) =>
{
    var res = await authService.UpdateUserAsync(userName,password);
    return Results.Ok("Detail successfully updated");
}
);

app.MapDelete("/DeleteUser", async ([FromBody]string username,IAuthService authService) =>
{
    var res = await authService.DeleteUserAsync(username);

    if (res is false) return Results.NotFound();

    return Results.NoContent();

});

app.MapDefaultEndpoints();

app.Run();
