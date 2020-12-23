using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Contratos.Validator;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Exceptions;

namespace DV.FrenteLoja.Core.Servicos
{
	public class ClienteServico : IClienteServico
	{
		private readonly IRepositorio<Cliente> _clienteRepositorio;
		private readonly ClienteValidator _clienteValidator;
		public ClienteServico(IRepositorioEscopo escopo)
		{
			_clienteRepositorio = escopo.GetRepositorio<Cliente>();
			_clienteValidator = new ClienteValidator();
		}

/*		public ClienteDto ObterClientePorId(long id)
		{            
			var cliente = _clienteRepositorio.GetSingle(x => !x.RegistroInativo && x.Id == id);
			if (cliente == null)
				throw new NegocioException("Cliente não encontrado");

			var clienteDto = Mapper.Map<ClienteDto>(cliente);

			var validationResult = _clienteValidator.Validate(clienteDto);
			if (validationResult.IsValid)
			{
				return clienteDto;
			}
			throw new NegocioException(string.Empty, validationResult.Errors);
		}
*/
/*		public ClienteDto ObterClientePorCnpj(string cnpj)
		{
			var cliente = _clienteRepositorio.GetSingle(x => !x.RegistroInativo && x.CNPJCPF.Equals(cnpj, StringComparison.InvariantCultureIgnoreCase));
			if (cliente == null)
				throw new NegocioException("Cliente não encontrado");

			var clienteDto = Mapper.Map<ClienteDto>(cliente);

			var validationResult = _clienteValidator.Validate(clienteDto);
			if (validationResult.IsValid)
			{
				return clienteDto;
			}
			throw new NegocioException(string.Empty, validationResult.Errors);
		}
*/
/*        public ClienteDto ObterClientePorCpf(string cpf)
        {
            var cliente = _clienteRepositorio.GetSingle(x => !x.RegistroInativo &&
                x.CNPJCPF.Equals(cpf, StringComparison.InvariantCultureIgnoreCase) && x.Loja == "99"); // No Protheus quando é cpf a Loja é sempre 99.
            if (cliente == null)
                throw new NegocioException("Cliente não encontrado");

            var clienteDto = Mapper.Map<ClienteDto>(cliente);

            var validationResult = _clienteValidator.Validate(clienteDto);
            if (validationResult.IsValid)
            {
                return clienteDto;
            }
            throw new NegocioException(string.Empty, validationResult.Errors);
        }
*/
/*        public ClienteDto ObterClientePorCodigo(string codigo)
		{
			if (!codigo.Contains("-") || codigo.Length != 9)
				throw new NegocioException("Código está em um formato incorreto. Formato esperado: 000000-00.");

			var campoCodigo = codigo.Split('-')[0];
			var loja = codigo.Split('-')[1];

			Cliente cliente;
			if (!string.IsNullOrEmpty(loja) && !loja.Equals("00", StringComparison.InvariantCultureIgnoreCase))
			{
				cliente = _clienteRepositorio.GetSingle(x => !x.RegistroInativo && x.Loja.Equals(loja, StringComparison.InvariantCultureIgnoreCase)
				&& x.CampoCodigo.Equals(campoCodigo, StringComparison.InvariantCultureIgnoreCase));

				if (cliente == null)
					throw new NegocioException("Cliente não encontrado");
			}
			else
			{
				cliente = _clienteRepositorio.GetSingle(x => !x.RegistroInativo && x.CampoCodigo.Equals(campoCodigo, StringComparison.InvariantCultureIgnoreCase));

				if (cliente == null)
					throw new NegocioException("Cliente não encontrado");
			}


			var clienteDto = Mapper.Map<ClienteDto>(cliente);

			var validationResult = _clienteValidator.Validate(clienteDto);
			if (validationResult.IsValid)
			{
				return clienteDto;
			}

			throw new NegocioException(string.Empty, validationResult.Errors);
		}
*/
		public ClienteDto ObterClientePorNome(string nome)
		{
			if (string.IsNullOrEmpty(nome) || nome.Length < 6)
				throw new NegocioException("Formato incorreto.");

			var cliente = _clienteRepositorio.GetSingle(x => !x.RegistroInativo && x.Nome.Equals(nome, StringComparison.InvariantCultureIgnoreCase));
			if (cliente == null)
				throw new NegocioException("Cliente não encontrado");

			var clienteDto = Mapper.Map<ClienteDto>(cliente);

			var validationResult = _clienteValidator.Validate(clienteDto);
			if (validationResult.IsValid)
			{
				return clienteDto;
			}
			throw new NegocioException(string.Empty, validationResult.Errors);
		}

/*		public int TamanhoClientePorTermo(string termoBusca)
		{
			return ObterClienteQuery(termoBusca).Count();
		}
*/
/*		public List<ClienteDto> ObterClientePorNome(string termoBusca, int tamanhoPagina, int numeroPagina)
		{
		    termoBusca = string.Format("{0}%", termoBusca).ToLower();
            var clientes = ObterClienteQuery(termoBusca)
                           .Take(tamanhoPagina).ToList();

            return Mapper.Map<List<ClienteDto>>(clientes);
        }
*/
/*		private IQueryable<Cliente> ObterClienteQuery(string termoBusca)
		{
           // termoBusca = string.Format("{0}%", termoBusca);
            return from cliente in _clienteRepositorio.GetAll()
				where DbFunctions.Like(cliente.Nome, termoBusca) && !cliente.RegistroInativo
				orderby cliente.Id
				select cliente;
		}
        */
	}
}