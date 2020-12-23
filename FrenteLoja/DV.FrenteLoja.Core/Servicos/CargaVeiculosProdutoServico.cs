using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Infra.EntityFramework;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Servicos.Comuns;

namespace DV.FrenteLoja.Core.Servicos
{
    public class CargaVeiculosProdutoServico : ICargaVeiculoProdutosServico
    {
        private readonly DellaviaContexto _contexto;
        private LogIntegracao _logIntegracao;

        private const string UsuarioAtualizacaoServico = "Serviço de Atualização";

        private readonly IRepositorioEscopo _escopo;
        private readonly IRepositorio<LogIntegracao> _logIntegracaoRepositorio;
        private readonly IRepositorio<Veiculo> _veiculoRepositorio;
        private readonly IRepositorio<VwVeiculoProdutos> _vwVeiculosProdutoRepositorio;
        private readonly IRepositorio<Catalogo> _catalogoRepositorio;
        private readonly IRepositorio<Produto> _produtoRepositorio;
        private readonly IRepositorio<MarcaModeloVersao> _marcaModeloVersaoRepositorio;
        private readonly IRepositorio<MarcaModelo> _marcaModeloRepositorio;
        private readonly IRepositorio<Marca> _marcaRepositorio;
        private readonly IRepositorio<VersaoMotor> _versaoMotorRepositorio;

        public CargaVeiculosProdutoServico(IRepositorioEscopo escopo, DellaviaContexto contexto)
        {
            _escopo = escopo;
            _contexto = contexto;
            _logIntegracaoRepositorio = escopo.GetRepositorio<LogIntegracao>();
            _veiculoRepositorio = escopo.GetRepositorio<Veiculo>();
            _vwVeiculosProdutoRepositorio = escopo.GetRepositorio<VwVeiculoProdutos>();
            _catalogoRepositorio = escopo.GetRepositorio<Catalogo>();
            _produtoRepositorio = escopo.GetRepositorio<Produto>();
            _marcaModeloVersaoRepositorio = escopo.GetRepositorio<MarcaModeloVersao>();
            _marcaModeloRepositorio = escopo.GetRepositorio<MarcaModelo>();
            _marcaRepositorio = escopo.GetRepositorio<Marca>();
            _versaoMotorRepositorio = escopo.GetRepositorio<VersaoMotor>();
        }

        public async Task SyncProdutos()
        {
            _logIntegracao = new LogIntegracao()
            {
                DataAtualizacao = DateTime.Now,
                UsuarioAtualizacao = UsuarioAtualizacaoServico
            };

            var veiculos = (from v in _veiculoRepositorio.GetAll()
                            where v.IdFraga != null
                            select v).ToList();

            foreach (Veiculo veiculo in veiculos)
            {
                try
                {
                    var produtos = (from p in _produtoRepositorio.GetAll()
                                    join vwvp in _vwVeiculosProdutoRepositorio.GetAll() on p.CodigoFabricante equals vwvp.PartNumber
                                    join c in _catalogoRepositorio.GetAll() on vwvp.PartNumber equals c.CodigoFabricante into fabricantes
                                    where vwvp.Veiculo_Id == veiculo.IdFraga && fabricantes.Count() == 0
                                    select p).ToList().Distinct();

                    foreach (Produto produto in produtos)
                    {
                        try
                        {
                            using (DbContextTransaction dbContext = _contexto.Database.BeginTransaction())
                            {
                                MarcaModeloVersao versao = _marcaModeloVersaoRepositorio.Get(a => a.Id == veiculo.MarcaModeloVersao.Id).First();
                                MarcaModelo modelo = _marcaModeloRepositorio.Get(a => a.Id == versao.IdMarcaModelo).First();
                                Marca marca = _marcaRepositorio.Get(a => a.Id == modelo.IdMarca).First();
                                VersaoMotor versaoMotor = _versaoMotorRepositorio.Get(a => a.Id == veiculo.VersaoMotor.Id).First();
                                Catalogo catalogo = new Catalogo()
                                {
                                    CodigoFabricante = produto.CodigoFabricante,
                                    FabricantePeca = produto.FabricantePeca,
                                    MarcaVeiculo = marca.Descricao,
                                    ModeloVeiculo = modelo.Descricao,
                                    VersaoVeiculo = versao.Descricao,
                                    VersaoMotor = versaoMotor.Descricao,
                                    AnoInicial = veiculo.AnoInicial.Year,
                                    AnoFinal = veiculo.AnoFinal.Year,
                                    CodigoDellavia = produto?.CampoCodigo,
                                    Descricao = produto.Descricao,
                                    DataAtualizacao = DateTime.Now,
                                    UsuarioAtualizacao = UsuarioAtualizacaoServico
                                };
                                _catalogoRepositorio.Add(catalogo);

                                try
                                {
                                    Elasticsearch elastic = new Elasticsearch(_escopo);
                                    elastic.EnviarElasticsearch(produto, veiculo);
                                    _logIntegracao.StatusIntegracao = StatusIntegracao.Sucesso;
                                }
                                catch (Exception e)
                                {
                                    _logIntegracao.Log = $"Erro ao enviar para o Elasticsearch. Erro: {e}";
                                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                                    dbContext.Rollback();
                                }
                                dbContext.Commit();
                            }
                        }
                        catch (Exception e)
                        {
                            _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                            _logIntegracao.Log = $"Ocorreu erro inserir registro no catálogo. Erro: {e}";
                        }
                    }
                }
                catch (Exception e)
                {
                    _logIntegracao.StatusIntegracao = StatusIntegracao.Erro;
                    _logIntegracao.Log = $"Não foi possível importar o veículo {veiculo.Id}. Erro: {e}";
                }
                finally
                {
                    _logIntegracaoRepositorio.Add(_logIntegracao);
                }
            }
        }
    }
}
