using System;
using System.Data.Entity;
using System.Web;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Extensions;
using DV.FrenteLoja.Core.Infra.EntityFramework;
using DV.FrenteLoja.Core.Infra.Security;

namespace DV.FrenteLoja.Core.Servicos
{
    public class HomeServico : IHomeServico
    {
        private readonly DellaviaContexto _contexto;
        private readonly IRepositorio<Orcamento> _orcamentoRepositorio;
        private readonly IRepositorio<LojaDellaVia> _lojaRepositorio;
        private readonly IRepositorio<Checkup> _checkupRepositorio;


        public HomeServico(DellaviaContexto contexto)
        {
            _contexto = contexto;
            _orcamentoRepositorio = contexto.GetRepository<Orcamento>();
            _checkupRepositorio = contexto.GetRepository<Checkup>();
            _lojaRepositorio = contexto.GetRepository<LojaDellaVia>();
        }

        public HomeDto ObterInformacoesIniciais()
        {
            HomeDto homeDto = new HomeDto();
            homeDto.NomeUsuario = HttpContext.Current.User.Identity.GetName();
            string periodo = "Boa Noite";


            if (DateTime.Now.Hour < 12)
                periodo = "Bom dia";

            if (DateTime.Now.Hour >= 12 && DateTime.Now.Hour < 18)
                periodo = "Boa tarde";


            homeDto.PeriodoAtual = periodo;

            var campoIdLoja = HttpContext.Current.User.Identity.GetLojaPadrao();

            var checkupCount = _checkupRepositorio.Count(x => DbFunctions.TruncateTime(x.DataAtualizacao) == DbFunctions.TruncateTime(DateTime.Now)
                                         && campoIdLoja.Contains(x.Orcamento.LojaDellaVia.CampoCodigo));

            homeDto.QuantidadeCheckups = checkupCount;

            var user = HttpContext.Current.User.Identity.Name.ToUpper();

            var orcamentoCount = _orcamentoRepositorio.Count(x => x.UsuarioAtualizacao == user
            && DbFunctions.TruncateTime(x.DataCriacao) == DbFunctions.TruncateTime(DateTime.Now));

            homeDto.QuantidadeOrcamentosPorUsuario = orcamentoCount;

            var orcamentoVencendoCount =
                _orcamentoRepositorio.Count(x => x.UsuarioAtualizacao == user 
                && DbFunctions.TruncateTime(x.DataValidade) == DbFunctions.TruncateTime(DateTime.Now));

            homeDto.QuantidadeOrcamentosVencendo = orcamentoVencendoCount;

            return homeDto;
        }
    }
}