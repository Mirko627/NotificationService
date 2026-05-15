using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NotificationService.Business.Interfaces;
using NotificationService.Business.Mappers;
using NotificationService.Data.Context;
using NotificationService.Data.Repositories;
using NotificationService.Repository.Interfaces;
using System.Text;
using UserService.ClientHttp.Clients;
using UserService.ClientHttp.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NotificationDB") ?? throw new InvalidOperationException("Connection string 'NotificationDB' not found.")));

string? userServiceUrl = builder.Configuration["ExternalServices:UserServiceUrl"];

if (string.IsNullOrEmpty(userServiceUrl))
{
    throw new Exception("L'URL di UserService non � configurato nel file appsettings.json");
}
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<IUserClient, UserClient>(client =>
{
    client.BaseAddress = new Uri(userServiceUrl);
});
// AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<VisitMapper>();
});
builder.Services.AddAuthentication(options =>
{
    // Impostiamo JWT come schema predefinito
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "UserService", // Deve corrispondere a quello del token
        ValidAudience = "ProjectMicroservizi", // Deve corrispondere
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("b133a0c0e9bee3be20163d2ad31d6248db292aa6dcb1ee087a2aa50e0fc75ae2"))
    };
});
// Services e repository
builder.Services.AddScoped<INotificationService, NotificationService.Business.Services.NotificationService>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Notification Service API", Version = "v1" });

    // Aggiunge la definizione dello schema di sicurezza (il lucchetto)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Inserisci il token JWT nel formato: Bearer {tuo_token}"
    });

    // Rende obbligatorio il lucchetto per le chiamate
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
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
