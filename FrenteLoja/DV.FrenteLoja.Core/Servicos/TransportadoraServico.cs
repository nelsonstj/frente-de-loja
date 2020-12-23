using AutoMapper.QueryableExtensions;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DV.FrenteLoja.Core.Servicos
{
    public class TransportadoraServico: ITransportadoraServico
	{
		private readonly IRepositorio<Transportadora> _transportadoraRepositorio;
		public TransportadoraServico(IRepositorioEscopo escopo)
		{
			_transportadoraRepositorio = escopo.GetRepositorio<Transportadora>();
		}
		public int TamanhoTransportadoraPorTermo(string termoBusca)
		{
			return ObterTransportadorasQuery(termoBusca).Count();
		}

		public List<TransportadoraDto> ObterTransportadorasPorNome(string termoBusca, int tamanhoPagina, int numeroPagina)
		{
			return ObterTransportadorasQuery(termoBusca)
				.Skip(tamanhoPagina * (numeroPagina - 1))
				.Take(tamanhoPagina)
				.ProjectTo<TransportadoraDto>()
				.ToList();
		}

		private IQueryable<Transportadora> ObterTransportadorasQuery(string termoBusca)
		{
            termoBusca = string.Format("{0}%", termoBusca).ToLower();

			return from transportadora in _transportadoraRepositorio.GetAll()
                   where (DbFunctions.Like(transportadora.Descricao, termoBusca) ||
                          DbFunctions.Like(transportadora.CampoCodigo, termoBusca))
                         && !transportadora.RegistroInativo
                   orderby transportadora.CampoCodigo
                   select transportadora;
		}
	}
}