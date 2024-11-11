using Core.Interfaces;
using Core.Repositories;
using Core.Services;
using Core.Mapping;
using System.Data;
using Microsoft.Data.Sqlite; // Importação para o Sqlite
using AutoMapper;
using Dapper;



var builder = WebApplication.CreateBuilder(args);

SQLitePCL.Batteries.Init(); // Inicializa a biblioteca SQLite




// Adiciona os controladores ao contêiner
builder.Services.AddControllers();

// Configuração do Swagger para geração de documentação da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);  // Inclui os comentários XML para Swagger
});

// Configuração de Dapper com SQLite
builder.Services.AddScoped<IDbConnection>(sp => new SqliteConnection(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

// Configuração do AutoMapper e injeção de dependências de Repositórios e Serviços
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));  // Registro do perfil AutoMapper
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IMedicoRepository, MedicoRepository>();
builder.Services.AddScoped<IConsultaRepository, ConsultaRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IMedicoService, MedicoService>();
builder.Services.AddScoped<IConsultaService, ConsultaService>();

var app = builder.Build();

// Configuração do middleware para o ambiente de desenvolvimento
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

