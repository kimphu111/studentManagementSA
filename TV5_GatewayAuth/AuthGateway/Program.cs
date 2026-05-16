using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using AuthGateway.Data;
using AuthGateway.Services;


var builder = WebApplication.CreateBuilder(args);

//.env
DotNetEnv.Env.Load();

builder.Services.AddControllers();

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//JWT Service
builder.Services.AddScoped<JwtService>();

//DB (MySql)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

//CORS for FE Login
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

//JWT Authentication
builder.Services.AddAuthentication("Bearer")
   .AddJwtBearer("Bearer", options =>
   {
       var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");

       if (string.IsNullOrEmpty(jwtKey))
       {
           throw new Exception("JWT_KEY is missing in environment variables");
       }

       options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
       {
           ValidateIssuer = false,
           ValidateAudience = false,
           ValidateIssuerSigningKey = true,
           IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
               System.Text.Encoding.UTF8.GetBytes(jwtKey))
       };
   });

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5157, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//await app.UseOcelot();

app.Run();
