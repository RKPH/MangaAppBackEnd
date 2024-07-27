using System.Text;
using MangaApp.Data;
using MangaApp.Interfaces;
using MangaApp.Services;
using MangaApp.Respository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CloudinaryDotNet;


var builder = WebApplication.CreateBuilder(args);
var _config = builder.Configuration;

// Configure Cloudinary account
var cloudinaryAccount = new Account(
    _config["Cloudinary:CloudName"],
    _config["Cloudinary:ApiKey"],
    _config["Cloudinary:ApiSecret"]
);
var cloudinary = new CloudinaryDotNet.Cloudinary(cloudinaryAccount);
builder.Services.AddSingleton(cloudinary);

// Add services to the container
builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"])),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("role", "Admin"));
});

builder.Services.AddDbContext<MangaAppDbcontext>(options =>
    options.UseNpgsql(_config.GetConnectionString("DefaultConnection")));

// Register application services
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IUserMangaRepository, UserMangaRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IGachaRepository, GachaRepository>();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy =>
    policy.WithOrigins("http://localhost:5173", "http://localhost:3000", "http://hung11062003-001-site1.btempurl.com/", "https://manga-app-steel.vercel.app")
        .AllowAnyMethod()
        .AllowAnyHeader());

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Configure port binding
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://*:{port}");

app.Run();
