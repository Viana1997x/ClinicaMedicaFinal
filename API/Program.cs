using Core.Interfaces;
using Core.Repositories;
using Core.Services;
using Core.Mapping;
using System.Data;
using Microsoft.Data.Sqlite; // Importa��o para o Sqlite
using AutoMapper;
using Dapper;



var builder = WebApplication.CreateBuilder(args);

SQLitePCL.Batteries.Init(); // Inicializa a biblioteca SQLite




// Adiciona os controladores ao cont�iner
builder.Services.AddControllers();

// Configura��o do Swagger para gera��o de documenta��o da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);  // Inclui os coment�rios XML para Swagger
});

// Configura��o de Dapper com SQLite
builder.Services.AddScoped<IDbConnection>(sp => new SqliteConnection(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

// Configura��o do AutoMapper e inje��o de depend�ncias de Reposit�rios e Servi�os
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));  // Registro do perfil AutoMapper
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IMedicoRepository, MedicoRepository>();
builder.Services.AddScoped<IConsultaRepository, ConsultaRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IMedicoService, MedicoService>();
builder.Services.AddScoped<IConsultaService, ConsultaService>();

var app = builder.Build();

// Configura��o do middleware para o ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

