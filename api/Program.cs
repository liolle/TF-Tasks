using System.Text;
using apiExo.dal.database;
using apiExo.bll.services;
using DotNetEnv;
using apiExo.bll.entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

Env.Load();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

// Modify config variable that will be injected by Services.AddSingleton<IConfiguration>(configuration)
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);


//JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options=>{
        string? jwt_key = configuration["JWT_KEY"] ?? throw new Exception("Missing JWT_KEY configuration");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["JWT_ISSUER"],
            ValidAudience = configuration["JWT_AUDIENCE"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt_key))
        };


        // extract token from cookies and place it into the Bearer.
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                string? jwt_name = configuration["AUTH_TOKEN_NAME"] ?? throw new Exception("Missing AUTH_TOKEN_NAME configuration");
                if (context.Request.Cookies.ContainsKey(jwt_name))
                {
                    context.Token = context.Request.Cookies[jwt_name];
                }
                return Task.CompletedTask;
            }
        };
    });

// CORS 
builder.Services.AddCors(options=>{
    options.AddPolicy("auth-input", policy=>{
        policy
        .WithOrigins(["http://localhost:3000","http://localhost"])
        .AllowCredentials()
        .WithHeaders(Microsoft.Net.Http.Headers.HeaderNames.ContentType, "Authorization")
        .WithMethods(["POST","OPTIONS"]);
    });
});

// BLL services
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));

builder.Services.AddScoped<IDataContext,DataContext>();
builder.Services.AddTransient<IHashService,HashService>();
builder.Services.AddTransient<IUserService,UserService>();
builder.Services.AddTransient<ITaskService,TaskService>();
builder.Services.AddControllers();
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
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

RouteConfig.RegisterRoutes(app);

app.Run();