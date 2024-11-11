using Core.Entities;
using Core.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly IDbConnection _dbConnection;

        public ClienteRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> RegistrarCliente(Cliente cliente)
        {
            string sql = "INSERT INTO Clientes (Nome, CPF, Endereco, DataNascimento, Telefone, Senha) VALUES (@Nome, @CPF, @Endereco, @DataNascimento, @Telefone, @Senha)";
            return await _dbConnection.ExecuteAsync(sql, cliente);
        }

        public async Task<Cliente> LoginCliente(string cpf, string senha)
        {
            string sql = "SELECT * FROM Clientes WHERE CPF = @CPF AND Senha = @Senha";
            return await _dbConnection.QueryFirstOrDefaultAsync<Cliente>(sql, new { CPF = cpf, Senha = senha });
        }

        public async Task<IEnumerable<Consulta>> ObterConsultasPorCliente(int clienteId)
        {
            string sql = "SELECT * FROM Consultas WHERE ClienteId = @ClienteId";
            return await _dbConnection.QueryAsync<Consulta>(sql, new { ClienteId = clienteId });
        }
    }

}
