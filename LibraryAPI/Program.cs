using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Text;
using LibraryAPI.Controllers;

// Create a new web application builder.
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Library API", Version = "v1" });

    c.IncludeXmlComments(AppDomain.CurrentDomain.BaseDirectory + "LibraryAPI.xml");
    // Configure JWT authentication for Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new List<string>()
        }
    });
});

// Add the LibraryManagementSystemContext to the container.
builder.Services.AddDbContext<LibraryManagementSystemContext>();

// Register the MemberServices as Scoped.
builder.Services.AddScoped<MemberServices>();


// Generate a random secret key for JWT and set it in the configuration.
var keySizeInBytes = 32;
var secretKey = AuthController.GenerateRandomKey();
builder.Configuration["Jwt:SecretKey"] = secretKey;

// Configure JWT authentication.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = "https://library.example.com/", // Replace with your issuer URL or name
               ValidAudience = "https://api.example.com/", // Replace with your audience URL or name
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)) // Replace with your secret key
           };
       });

// Build the application.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API V1");

        c.OAuthClientId("Swagger");
        c.OAuthAppName("Swagger UI");
    });
}

app.UseRouting();

// Add the ErrorMiddleware to handle exceptions.
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Run the application.
app.Run();
