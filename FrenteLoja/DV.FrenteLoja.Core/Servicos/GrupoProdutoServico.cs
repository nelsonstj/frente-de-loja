using System.Linq;
using AutoMapper;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using System.Data.Entity;
using DV.FrenteLoja.Core.Contratos.Enums;

namespace DV.FrenteLoja.Core.Servicos
{
    public class GrupoProdutoServico : IGrupoProdutoServico
    {
        private readonly IRepositorio<GrupoProduto> _repositorioGrupoProduto;
        private readonly IMapper mapper;


        public GrupoProdutoServico(IRepositorioEscopo escopo)
        {
            _repositorioGrupoProduto = escopo.GetRepositorio<GrupoProduto>();
        }


        public TipoProdutoElasticSearch BuscaTipoProdutoElasticSearch(string campoCodigoGrupoProduto)
        {
            var retorno = from grupoProduto in _repositorioGrupoProduto.GetAll().Include("GrupoSubGrupo")
                          where (DbFunctions.Like(grupoProduto.CampoCodigo, campoCodigoGrupoProduto))
                          orderby grupoProduto.CampoCodigo
                          select grupoProduto.GrupoSubGrupo.Grupo;
            return retorno.FirstOrDefault();
        }
    }
}
