using Core.Interfaces;
using Core.Repositories;
using Core.Services;
using Core.Mapping;
using System.Data;
using Microsoft.Data.Sqlite;
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

// M�todo para garantir que o banco de dados SQLite esteja criado e inicializado corretamente
InitializeDatabase(app.Services);

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

/// <summary>
/// M�todo para inicializar o banco de dados SQLite
/// </summary>
/// <param name="services"></param>
void InitializeDatabase(IServiceProvider services)
{
    using (var scope = services.CreateScope())
    {
        var scopedServices = scope.ServiceProvider;
        using var connection = scopedServices.GetRequiredService<IDbConnection>();

        // Cria��o de tabela de exemplo para Clientes (adapte conforme sua necessidade)
        var clienteTable = @"
            CREATE TABLE IF NOT EXISTS Clientes (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nome TEXT NOT NULL,
                CPF TEXT NOT NULL,
                Endereco TEXT NOT NULL,
                DataNascimento TEXT NOT NULL,
                Telefone TEXT NOT NULL,
                Senha TEXT NOT NULL
            );";

        connection.Execute(clienteTable);

        // Cria��o de tabela de exemplo para M�dicos (adapte conforme sua necessidade)
        var medicoTable = @"
            CREATE TABLE IF NOT EXISTS Medicos (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nome TEXT NOT NULL,
                CPF TEXT NOT NULL,
                Endereco TEXT NOT NULL,
                DataNascimento TEXT NOT NULL,
                Telefone TEXT NOT NULL,
                AreaEspecializacao TEXT NOT NULL,
                Senha TEXT NOT NULL
            );";

        connection.Execute(medicoTable);

        // Cria��o de tabela de exemplo para Consultas
        var consultaTable = @"
            CREATE TABLE IF NOT EXISTS Consultas (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ClienteId INTEGER,
                MedicoId INTEGER,
                DataConsulta TEXT NOT NULL,
                FOREIGN KEY(ClienteId) REFERENCES Clientes(Id),
                FOREIGN KEY(MedicoId) REFERENCES Medicos(Id)
            );";

        connection.Execute(consultaTable);
    }
}