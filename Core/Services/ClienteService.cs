using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

        public ClienteService(IClienteRepository clienteRepository, IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
        }

        public async Task<int> RegistrarCliente(ClienteDTO clienteDTO)
        {
            var cliente = _mapper.Map<Cliente>(clienteDTO);
            return await _clienteRepository.RegistrarCliente(cliente);
        }

        public async Task<ClienteDTO> LoginCliente(string cpf, string senha)
        {
            var cliente = await _clienteRepository.LoginCliente(cpf, senha);
            return _mapper.Map<ClienteDTO>(cliente);
        }

        public async Task<IEnumerable<ConsultaDTO>> ObterConsultasPorCliente(int clienteId)
        {
            var consultas = await _clienteRepository.ObterConsultasPorCliente(clienteId);
            return _mapper.Map<IEnumerable<ConsultaDTO>>(consultas);
        }
    }

}
