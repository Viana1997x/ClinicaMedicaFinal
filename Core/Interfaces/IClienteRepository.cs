using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IClienteRepository
    {
        Task<int> RegistrarCliente(Cliente cliente);
        Task<Cliente> LoginCliente(string cpf, string senha);
        Task<IEnumerable<Consulta>> ObterConsultasPorCliente(int clienteId);
    }

}
