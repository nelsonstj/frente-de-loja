using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Servicos
{
    public class ImpostosServico : IImpostosServico
    {
        private readonly IRepositorioEscopo _escopo;
        private readonly IRepositorio<Orcamento> _orcamentoRepositorio;
        private readonly ICalculoImpostosApi _calculoImpostosApi;

        public ImpostosServico(IRepositorioEscopo escopo, ICalculoImpostosApi calculoImpostosApi)
        {
            _escopo = escopo;
            _orcamentoRepositorio = escopo.GetRepositorio<Orcamento>();
            _calculoImpostosApi = calculoImpostosApi;
        }
        public void CalcularImpostos(Orcamento orcamento)
        {
            var orc = orcamento;

            var valorImpostos = _calculoImpostosApi.CalculoImpostos(orc);
            orc.ValorImpostos = valorImpostos;
            _escopo.Finalizar();
        }

        public void CalcularImpostos(long idOrcamento)
        {

            var orc = _orcamentoRepositorio.GetSingle(x => x.Id == idOrcamento);
            var valorImpostos = decimal.Zero;
            if (orc.OrcamentoItens != null)
                valorImpostos = _calculoImpostosApi.CalculoImpostos(orc);
            orc.ValorImpostos = valorImpostos;
            _escopo.Finalizar();

        }
    }
}