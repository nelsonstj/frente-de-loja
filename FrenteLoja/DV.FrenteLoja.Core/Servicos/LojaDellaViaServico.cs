using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Infra.Security;
using System.Data.Entity;

namespace DV.FrenteLoja.Core.Servicos
{
	public class LojaDellaViaServico:ILojaDellaViaServico
	{
		private readonly IRepositorio<LojaDellaVia> _lojaDellaviaRepositorio;
		private readonly IRepositorio<LojaDellaViaProxima> _lojaDellaviaProximaRepositorio;


		public LojaDellaViaServico(IRepositorioEscopo escopo)
		{
			_lojaDellaviaRepositorio = escopo.GetRepositorio<LojaDellaVia>();
			_lojaDellaviaProximaRepositorio = escopo.GetRepositorio<LojaDellaViaProxima>();
		}
		public int TamanhoLojasPorTermo(string termoBusca)
		{
			return ObterLojasQuery(termoBusca).Count();
		}

		public List<LojaDellaViaDto> ObterLojasPorNome(string termoBusca, int tamanhoPagina, int numeroPagina)
		{
			termoBusca = termoBusca.ToLower();

			return ObterLojasQuery(termoBusca)
				.Skip(tamanhoPagina * (numeroPagina - 1))
				.Take(tamanhoPagina)
				.ProjectTo<LojaDellaViaDto>()
				.ToList();
		}

		public LojaDellaViaDto ObterLojaUsuarioLogado()
		{
			var lojaCodigo = HttpContext.Current.User.Identity.GetLojaPadrao();
			var loja = _lojaDellaviaRepositorio.GetSingle(x => !x.RegistroInativo && lojaCodigo.Contains(x.CampoCodigo));
			if(loja == null)
				throw new Exception("Loja não encontrada no perfil logado do usuario.");

			return Mapper.Map<LojaDellaViaDto>(loja);
		}

        public List<LojaDellaViaDto> ObterLojasProximas(long iDLojaDellavia)
        {
            var lojasProximas = _lojaDellaviaProximaRepositorio.Get(x => x.IdLojaDellaViaReferencia == iDLojaDellavia);

            var lojas = new List<LojaDellaVia>();
            lojas.Add(new LojaDellaVia { Id = iDLojaDellavia });
            foreach (var lojaDellaViaProxima in lojasProximas)
            {
                var loja = _lojaDellaviaRepositorio.GetSingle(x => x.Id == lojaDellaViaProxima.Id);
                lojas.Add(loja);
            }

            return Mapper.Map<List<LojaDellaViaDto>>(lojas);

        }
            public List<LojaDellaViaDto> ObterLojasPorId(long id)
            {
                var lojas = _lojaDellaviaRepositorio.Get(x => x.Id == id);
            
                return Mapper.Map<List<LojaDellaViaDto>>(lojas);
            }

            private IQueryable<LojaDellaVia> ObterLojasQuery(string termoBusca)
		{
            termoBusca = string.Format("{0}%", termoBusca).ToLower();

            return from lojaDellaVia in _lojaDellaviaRepositorio.GetAll()
                where (DbFunctions.Like(lojaDellaVia.Descricao, termoBusca) ||
                       DbFunctions.Like(lojaDellaVia.CampoCodigo, termoBusca))
                      && !lojaDellaVia.RegistroInativo
				orderby lojaDellaVia.CampoCodigo
				select lojaDellaVia;

		}
	}
}