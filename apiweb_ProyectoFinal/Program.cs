using Configuracion;
using Datos;
using Datos.Interfaces.IDaos;
using Datos.Interfaces.IQuerys;
using Datos.Interfaces.IServicios;
using Datos.Interfaces.IValidaciones;
using Datos.Querys;
using Datos.Servicios;
using Datos.Validaciones;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sales API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. <br /> <br />
                      Enter 'Bearer' [space] and then your token in the text input below.<br /> <br />
                      Example: 'Bearer 12345abcdef'<br /> <br />",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,
            },
            new List<string>()
          }
        });
});

builder.Services.AddHttpContextAccessor();

//ID

builder.Services.AddScoped<IDaoBDUsuarios, DaoBDUsuarios>();
builder.Services.AddScoped<IDaoBDAdmins, DaoBDAdmins>();
builder.Services.AddScoped<IDaoBDPublicaciones, DaoBDPublicaciones>();
builder.Services.AddScoped<IDaoBDUsuarioAcceso, DaoBDUsuarioAcceso>();
builder.Services.AddScoped<IDaoBDCarrito, DaoBDCarrito>();
builder.Services.AddScoped<IDaoBDHistorias, DaoBDHistorias>();

builder.Services.AddScoped<IAdminServicios, AdminServicios>();
builder.Services.AddScoped<IUsuarioServicios, UsuarioServicios>();
builder.Services.AddScoped<IMetodosDeValidacion, MetodosDeValidacion>();
builder.Services.AddScoped<IPublicacionServicios, PublicacionServicios>();
builder.Services.AddScoped<ICarritoServicios, CarritoServicios>();
builder.Services.AddScoped<IHistoriaServicios, HistoriaServicios>();

builder.Services.AddScoped<IAccesoQuerys, AccesoQuerys>();
builder.Services.AddScoped<IAdminQuerys, AdminQuerys>();
builder.Services.AddScoped<IPublicacionQuerys, PublicacionQuerys>();
builder.Services.AddScoped<IUsuarioQuerys, UsuariosQuery>();
builder.Services.AddScoped<ICarritoQuerys, CarritoQuerys>();
builder.Services.AddScoped<IHistoriaQuerys,HistoriaQuerys>();

builder.Services.Configure<BDConfiguration>(builder.Configuration.GetSection("BD"));
builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("*");
        });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
    };
});

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

app.MapControllers();

app.Run();
