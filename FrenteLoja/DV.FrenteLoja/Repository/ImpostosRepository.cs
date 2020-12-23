// DV.FrenteLoja.Repository.ImpostosRepository
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Repository;

namespace DV.FrenteLoja.Repository
{
    public class ImpostosRepository
    {
        private readonly OrcamentoRepository _orcamentoRepository;
        private readonly ICalculoImpostosApi _calculoImpostosApi;

        public ImpostosRepository(ICalculoImpostosApi calculoImpostosApi, OrcamentoRepository orcamentoRepository)
        {
            _orcamentoRepository = orcamentoRepository;
            _calculoImpostosApi = calculoImpostosApi;
        }

        public void CalcularImpostos(Orcamento orcamento)
        {
            orcamento.ValorImpostos = _calculoImpostosApi.CalculoImpostos(orcamento);
            _orcamentoRepository.Update(orcamento);
        }


        public void CalcularImpostos(long idOrcamento)
        {

            var orc = _orcamentoRepository.GetOrcamentoById(idOrcamento);
            var valorImpostos = decimal.Zero;
            if (orc.OrcamentoItens != null)
                valorImpostos = _calculoImpostosApi.CalculoImpostos(orc);
            orc.ValorImpostos = valorImpostos;
            _orcamentoRepository.Update(orc);
        }
    }
}
