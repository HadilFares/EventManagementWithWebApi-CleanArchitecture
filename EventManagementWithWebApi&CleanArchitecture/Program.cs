using Application.Interfaces.Authentification;
using Infra.Data.Context;
using Infrastructure.Context;
using Infra.Data.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
/*builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));*/

builder.Services.AddDbContext<EventlyDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("Evently"),
    builder => builder.MigrationsAssembly("Infra.Data")));
builder.Services.AddDbContext<IdentityDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<EventlyDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddScoped<IAuthResponse, AuthResponseService>();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
