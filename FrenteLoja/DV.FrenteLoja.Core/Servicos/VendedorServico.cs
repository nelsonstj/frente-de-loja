using AutoMapper.QueryableExtensions;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DV.FrenteLoja.Core.Servicos
{
    public class VendedorServico:IVendedorServico
	{
		private readonly IRepositorio<Vendedor> _repositorioVendedor;
		public VendedorServico(IRepositorioEscopo escopo)
		{
			_repositorioVendedor = escopo.GetRepositorio<Vendedor>();
		}

		public int TamanhoVendedorPorTermo(string termoBusca)
		{
			return ObterVendedorQuery(termoBusca).Count();
		}
		public List<VendedorDto> ObterVendedorPorNome(string termoBusca, int tamanhoPagina, int numeroPagina)
		{
			return ObterVendedorQuery(termoBusca)
				.Skip(tamanhoPagina * (numeroPagina - 1))
				.Take(tamanhoPagina)
				.ProjectTo<VendedorDto>()
				.ToList();
		}
		private IQueryable<Vendedor> ObterVendedorQuery(string termoBusca)
		{
            termoBusca = string.Format("{0}%", termoBusca).ToLower();

            return from vendedor in _repositorioVendedor.GetAll()
				where (DbFunctions.Like(vendedor.Nome, termoBusca) ||
				       DbFunctions.Like(vendedor.CampoCodigo, termoBusca)) && !vendedor.RegistroInativo
				orderby vendedor.CampoCodigo
				select vendedor;
		}
	}
}