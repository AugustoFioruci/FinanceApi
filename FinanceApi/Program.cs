using FinanceApi.Data;
using FinanceApi.Exceptions;
using FinanceApi.Repositories;
using FinanceApi.Repositories.Interfaces;
using FinanceApi.Services;
using FinanceApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();  // ← adicionado

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAccountService, AccountService>();  // ← adicionado

// EF Core + PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT:Key não configurado.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };

        // ← LOGGING DETALHADO AQUI (adicione isso inteiro)
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                Console.WriteLine("[JWT] OnMessageReceived - Header recebido: " +
                    (context.Request.Headers["Authorization"].FirstOrDefault() ?? "Nenhum header Authorization"));
                return Task.CompletedTask;
            },

            OnTokenValidated = context =>
            {
                Console.WriteLine("[JWT] OnTokenValidated - Sucesso! UserId: " +
                    context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                return Task.CompletedTask;
            },

            OnAuthenticationFailed = context =>
            {
                var errorMsg = "[JWT] OnAuthenticationFailed - Motivo: " + context.Exception.Message;
                if (context.Exception.InnerException != null)
                {
                    errorMsg += "\nInner Exception: " + context.Exception.InnerException.Message;
                }
                Console.WriteLine(errorMsg);
                return Task.CompletedTask;
            },

            OnChallenge = context =>
            {
                Console.WriteLine("[JWT] OnChallenge - Enviando 401. Error: " +
                    (context.Error ?? "sem erro") +
                    " | Description: " + (context.ErrorDescription ?? "sem descrição"));
                return Task.CompletedTask;
            },

            OnForbidden = context =>
            {
                Console.WriteLine("[JWT] OnForbidden - Token válido mas sem permissão");
                return Task.CompletedTask;
            }
        };

        // Temporário: ajuda a ver se o token chega malformado
        options.IncludeErrorDetails = true;
    });

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            ValidAudience = builder.Configuration["Jwt:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
//            ClockSkew = TimeSpan.Zero  // mais seguro
//        };
//    });

builder.Services.AddAuthorization();

// Global Exception Handler (você terminou as exceptions, então ativa aqui)
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// CORS (opcional, mas útil para testes com frontend ou outros clientes)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// Swagger (deixo exatamente como você mandou, sem mudar NADA)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Finance API"
    });
    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header usando o esquema Bearer. Exemplo: \"Authorization: Bearer {token}\""
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [
            new OpenApiSecuritySchemeReference("bearer", document)
        ] = []
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");  // ← se quiser usar

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();  // ← ativa o handler global que você fez

app.MapControllers();

app.Run();