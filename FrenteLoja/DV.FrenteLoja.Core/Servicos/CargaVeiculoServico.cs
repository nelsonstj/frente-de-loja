using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Infra.EntityFramework;
using DV.FrenteLoja.Core.Servicos.Comuns;
using DV.FrenteLoja.Core.Util;

namespace DV.FrenteLoja.Core.Servicos
{
    public class CargaVeiculoServico : ICargaVeiculoServico
    {
        private const string UsuarioAtualizacaoServico = "Serviço de Atualização";

        private readonly IRepositorio<vwVeiculos> _vwVeiculosRepositorio;
        private readonly IRepositorio<VwVeiculoProdutos> _vwVeiculosProdutoRepositorio;
        private readonly IRepositorio<Veiculo> _veiculoRepositorio;
        private readonly IRepositorio<Marca> _marcaRepositorio;
        private readonly IRepositorio<MarcaModelo> _marcaModeloRepositorio;
        private readonly IRepositorio<MarcaModeloVersao> _marcaModeloVersaoRepositorio;
        private readonly IRepositorio<VersaoMotor> _versaoMotorRepositorio;
        private readonly IRepositorio<VeiculoMedidasPneus> _veiculoMedidasPneusRepositorio;
        private readonly DellaviaContexto _contexto;
        private readonly IRepositorio<LogIntegracao> _logIntegracaoRepositorio;
        private LogIntegracao _logIntegracao;
        private readonly IRepositorioEscopo _escopo;

        public CargaVeiculoServico(IRepositorioEscopo escopo, DellaviaContexto contexto)
        {
            _escopo = escopo;
            _contexto = contexto;
            _vwVeiculosRepositorio = escopo.GetRepositorio<vwVeiculos>();
            _vwVeiculosProdutoRepositorio = escopo.GetRepositorio<VwVeiculoProdutos>();
            _veiculoRepositorio = escopo.GetRepositorio<Veiculo>();
            _marcaRepositorio = escopo.GetRepositorio<Marca>();
            _marcaModeloRepositorio = escopo.GetRepositorio<MarcaModelo>();
            _marcaModeloVersaoRepositorio = escopo.GetRepositorio<MarcaModeloVersao>();
            _versaoMotorRepositorio = escopo.GetRepositorio<VersaoMotor>();
            _veiculoMedidasPneusRepositorio = escopo.GetRepositorio<VeiculoMedidasPneus>();
            _logIntegracaoRepositorio = escopo.GetRepositorio<LogIntegracao>();
        }

        public async Task SyncVeiculo()
        {
            _logIntegracao = new LogIntegracao()
            {
                DataAtualizacao = DateTime.Now,
                UsuarioAtualizacao = UsuarioAtualizacaoServico
            };
            
            var v = (from veiculo in _veiculoRepositorio.GetAll()
                     select veiculo.IdFraga).ToList();
            List<vwVeiculos> vwVeiculosList = (from vwVeiculos in _vwVeiculosRepositorio.GetAll()
                                               where !v.Contains(vwVeiculos.id)
                                               select vwVeiculos)
                                               .ToList();

            foreach (vwVeiculos vwVeiculo in vwVeiculosList)
            {
                using (DbContextTransaction dbContext = _contexto.Database.BeginTransaction())
                {
                    try
                    {
                        long marca = ConsultaBase.getIdMarca(_marcaRepositorio, vwVeiculo.Marca);
                        long modelo = ConsultaBase.getIdModelo(_marcaModeloRepositorio, marca, vwVeiculo.Modelo);
                        var veiculo = new Veiculo()
                        {
                            IdFraga = vwVeiculo.id.ToString(),
                            IdMarcaModeloVersao = ConsultaBase.getIdMarcaModeloVersao(_marcaModeloVersaoRepositorio, modelo, vwVeiculo.Versao),
                            AnoInicial = vwVeiculo.InicioProducao,
                            AnoFinal = vwVeiculo.FinalProducao,
                            IdVersaoMotor = ConsultaBase.getIdVersaoMotor(_versaoMotorRepositorio, vwVeiculo.VersaoMotor)
                        };
                        long veiculoId = _veiculoRepositorio.Add(veiculo).Id;

                        await SyncVeiculoProdutos(veiculoId, veiculo.IdFraga);

                        try
                        {
                            Elasticsearch elastic = new Elasticsearch(_escopo);
                            elastic.EnviarElasticsearch(veiculo);
                            _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;
                            dbContext.Commit();
                        }
                        catch (Exception e)
                        {
                            _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                            _logIntegracao.Log = $"Erro ao enviar para o Elasticsearch. Erro: {e}";
                            dbContext.Rollback();
                        }
                    }
                    catch (Exception e)
                    {
                        _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                        _logIntegracao.Log = $"Não foi possível importar o veículo {vwVeiculo.id}. Erro: {e}";
                        dbContext.Rollback();
                    }
                    finally
                    {
                        _logIntegracaoRepositorio.Add(_logIntegracao);
                    }
                }
            }
        }

        public async Task SyncVeiculoProdutos(long veiculoId, string IdFraga)
        {
            List<VwVeiculoProdutos> vwVeiculosProdutoList = (from vwVeiculosProduto in _vwVeiculosProdutoRepositorio.GetAll()
                                                            where vwVeiculosProduto.Veiculo_Id == IdFraga 
                                                               && vwVeiculosProduto.MarcaProduto.Contains("PNEU")
                                                           select vwVeiculosProduto)
                                                           .ToList();

            foreach (VwVeiculoProdutos vwVeiculosProduto in vwVeiculosProdutoList.Distinct().ToList())
            {
                decimal largura = Convert.ToDecimal(vwVeiculosProduto.PartNumber.Substring(0, 3).Replace(".", ","));
                decimal perfil = Convert.ToDecimal(vwVeiculosProduto.PartNumber.Substring(4, 2).Replace(".", ","));
                decimal aro = Convert.ToDecimal(vwVeiculosProduto.PartNumber.Substring(7, 2).Replace(".", ","));
                var posicao = vwVeiculosProduto.GrupoProduto.Contains("TRASEIRO")
                    ? VeiculoPosicaoPneuEnum.TRASEIRO
                    : VeiculoPosicaoPneuEnum.DIANTEIRO;

                VeiculoMedidasPneus veiculomedidas = (from vmpRepositorio in _veiculoMedidasPneusRepositorio.GetAll()
                                                     where vmpRepositorio.idVeiculo == veiculoId
                                                        && vmpRepositorio.Largura == largura
                                                        && vmpRepositorio.Perfil == perfil
                                                        && vmpRepositorio.Aro == aro
                                                        && vmpRepositorio.Posicao == posicao
                                                    select vmpRepositorio)
                                                    .FirstOrDefault();
                if (veiculomedidas == null)
                {
                    VeiculoMedidasPneus vmp = new VeiculoMedidasPneus()
                    {
                        idVeiculo = veiculoId,
                        Aro = aro,
                        Largura = largura,
                        Perfil = perfil,
                        Posicao = posicao,
                        UsuarioAtualizacao = UsuarioAtualizacaoServico,
                        CampoCodigo = IdFraga,
                        Indice = "0"
                    };
                    _veiculoMedidasPneusRepositorio.Add(vmp);
                }
            }
        }
    }       
}