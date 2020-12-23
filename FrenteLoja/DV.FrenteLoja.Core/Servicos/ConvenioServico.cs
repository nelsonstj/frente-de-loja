using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Exceptions;
using DV.FrenteLoja.Core.Infra.Security;
using System.Data.Entity;

namespace DV.FrenteLoja.Core.Servicos
{
    public class ConvenioServico : IConvenioServico
    {
        //private readonly IRepositorio<Convenio> _repositorioConvenio;
        private readonly IMapper mapper;


        public ConvenioServico(IRepositorioEscopo escopo)
        {
            //_repositorioConvenio = escopo.GetRepositorio<Convenio>();
        }


        /*public int TamanhoConveniosPorTermo(string termoBusca)
        {
            return ObterConvenioQuery(termoBusca).Count();
        }
        */

        /*public ConvenioDto ObterConvenioUsuarioLogado()
        {
            var convenioCodigo = HttpContext.Current.User.Identity.GetConvenioPadrao();
            var convenio = _repositorioConvenio.GetSingle(x => !x.RegistroInativo && x.CampoCodigo != null && x.CampoCodigo.Equals(convenioCodigo, StringComparison.CurrentCultureIgnoreCase));
            if (convenio == null)
                return null;
            return Mapper.Map<ConvenioDto>(convenio);
        }
        */

        /*public ConvenioDto ObterConvenioPorId(long id)
        {
            var convenio = _repositorioConvenio.GetSingle(x => !x.RegistroInativo && x.Id == id);
            if (convenio == null)
                throw new NegocioException("Convênio não encontrado.");

            return Mapper.Map<ConvenioDto>(convenio);
        }
        */

        /*public List<ConvenioDto> ObterConveniosPorTermo(string termoBusca, int tamanhoPagina, int numeroPagina)
        {
            termoBusca = termoBusca.ToLower();

            var a = ObterConvenioQuery(termoBusca)
                .Skip(tamanhoPagina * (numeroPagina - 1))
                .Take(tamanhoPagina)
                .ToList();

            return  Mapper.Map<List<ConvenioDto>>(a);
        }
        */

        /*private IQueryable<Convenio> ObterConvenioQuery(string termoBusca)
        {
            termoBusca = string.Format("{0}%", termoBusca).ToLower();

            return from convenio in _repositorioConvenio.GetAll()
                   where (DbFunctions.Like(convenio.Descricao, termoBusca) ||
                          DbFunctions.Like(convenio.CampoCodigo, termoBusca))
                    && !convenio.RegistroInativo
                   orderby convenio.CampoCodigo
                   select convenio;
        }*/
    }
}
