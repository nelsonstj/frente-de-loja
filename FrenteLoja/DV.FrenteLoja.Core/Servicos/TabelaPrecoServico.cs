using AutoMapper.QueryableExtensions;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DV.FrenteLoja.Core.Servicos
{
    public class TabelaPrecoServico: ITabelaPrecoServico
	{
		private readonly IRepositorio<TabelaPreco> _repositorioTabelaPreco;
		public TabelaPrecoServico(IRepositorioEscopo escopo)
		{
			
			_repositorioTabelaPreco = escopo.GetRepositorio<TabelaPreco>();
			
		}

		public int TamanhoTabelaPrecoPorTermo(string termoBusca)
		{
			return ObterTabelaPrecoQuery(termoBusca).Count();
		}

		
		public List<TabelaPrecoDto> ObterTabelaPrecoPorTermo(string termoBusca, int tamanhoPagina, int numeroPagina)
		{
			termoBusca = termoBusca.ToLower();

			return ObterTabelaPrecoQuery(termoBusca)
				.Skip(tamanhoPagina * (numeroPagina - 1))
				.Take(tamanhoPagina)
				.ProjectTo<TabelaPrecoDto>()
				.ToList();
		}

		private IQueryable<TabelaPreco> ObterTabelaPrecoQuery(string termoBusca)
		{
            termoBusca = string.Format("{0}%", termoBusca).ToLower();
            return from tabelaPreco in _repositorioTabelaPreco.GetAll()
				where (DbFunctions.Like(tabelaPreco.CampoCodigo, termoBusca))
                      && !tabelaPreco.RegistroInativo
				orderby tabelaPreco.Id
				select tabelaPreco;
		}
	}
}