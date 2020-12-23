using AutoMapper;
using AutoMapper.QueryableExtensions;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Infra.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DV.FrenteLoja.Core.Servicos
{
    public class TipoVendaServico : ITipoVendaServico
    {
        private readonly IRepositorioEscopo escopo;
        private readonly IRepositorio<TipoVenda> _repositorioTipoVenda;

        public TipoVendaServico(IRepositorioEscopo escopo)
        {
            this.escopo = escopo;
            _repositorioTipoVenda = escopo.GetRepositorio<TipoVenda>();
        }

        public List<AreaNegocioDto> ObterTipoVendaPorTermo(string termoBusca, int tamanhoPagina, int numeroPagina)
        {
            termoBusca = termoBusca.ToLower();

            return ObterTipoVendaQuery(termoBusca)
                .Skip(tamanhoPagina * (numeroPagina - 1))
                .Take(tamanhoPagina)
                .ProjectTo<AreaNegocioDto>()
                .ToList();
        }

	    public AreaNegocioDto ObterTipoVendaUsuarioLogado()
	    {
			var codigoTipoVenda =  HttpContext.Current.User.Identity.GetTipoVenda();
		    if (string.IsNullOrWhiteSpace(codigoTipoVenda))
			    return null;

		    var tipoVenda = _repositorioTipoVenda.GetSingle(x => !x.RegistroInativo && x.CampoCodigo.Equals(codigoTipoVenda, StringComparison.InvariantCultureIgnoreCase));

		    return Mapper.Map<AreaNegocioDto>(tipoVenda);

	    }

		private IQueryable<TipoVenda> ObterTipoVendaQuery(string termoBusca)
        {
            termoBusca = string.Format("{0}%", termoBusca).ToLower();

            return from tipoVenda in _repositorioTipoVenda.GetAll()
                   where (DbFunctions.Like(tipoVenda.Descricao, termoBusca) ||
                          DbFunctions.Like(tipoVenda.CampoCodigo, termoBusca))
                   && !tipoVenda.RegistroInativo
                   orderby tipoVenda.CampoCodigo
                   select tipoVenda; ;
        }

        public int QuantidadeTipoVendasPorTermo(string termoBusca)
        {
            return ObterTipoVendaQuery(termoBusca).Count();
        }
    }
}
