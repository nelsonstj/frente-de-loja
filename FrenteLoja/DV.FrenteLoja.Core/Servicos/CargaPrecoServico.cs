using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Extensions;
using DV.FrenteLoja.Core.Util;

namespace DV.FrenteLoja.Core.Servicos
{
    public class CargaPrecoServico : ICargaPrecoServico
    {
        private readonly IRepositorioEscopo _escopo;
        private readonly ICargaCadastrosProtheusSyncApi _protheusSyncApi;
        private readonly IRepositorio<TabelaPreco> _tabelaPrecoRepositorio;
        private readonly IRepositorio<TabelaPrecoItem> _tabelaPrecoItemRepositorio;
        private readonly IRepositorio<Produto> _produtoRepositorio;

        private const string UsuarioAtualizacaoServico = "Serviço de Atualização";


        public CargaPrecoServico(IRepositorioEscopo escopo, ICargaCadastrosProtheusSyncApi protheusSyncApi)
        {
            _escopo = escopo;
            _protheusSyncApi = protheusSyncApi;
            _tabelaPrecoRepositorio = escopo.GetRepositorio<TabelaPreco>();
            _produtoRepositorio = escopo.GetRepositorio<Produto>();
            _tabelaPrecoItemRepositorio = escopo.GetRepositorio<TabelaPrecoItem>();

        }
        public async Task SyncTabelaPreco(bool isFirstLoad = false)
        {
            var errosBuilder = new StringBuilder();

            var jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.TabelaPreco, TipoTabelaProtheus.DA0);
            var tabelaPrecos = new List<TabelaPreco>();

            foreach (var jObj in jArray)
            {
                var tabelaPreco = new TabelaPreco();
                try
                {
                    tabelaPreco.CampoCodigo = jObj["Id"].ToString();
                    tabelaPreco.DataDe = ProtheusConversions.ProtheusDate2DotNetDateHour(jObj["Data_Inicial"].ToString(), jObj["Hora_Inicial"].ToString()) ?? DateTime.MinValue;
                    tabelaPreco.DataAte = ProtheusConversions.ProtheusDate2DotNetDateHour(jObj["Data_Final"].ToString(), jObj["Hora_Final"].ToString()) ?? DateTime.MaxValue;
                    tabelaPreco.CodCondicaoPagamento = jObj["CondicaoPagamento_Id"].ToString();
                    tabelaPreco.Descricao = jObj["Descricao"].ToString();
                    tabelaPreco.RegistroInativo = !jObj["Ativo"].ToString().Equals("1");
                    tabelaPrecos.Add(tabelaPreco);
                }
                catch (Exception e)
                {
                    errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncTabelaPreco)} : {e}. Item: {tabelaPreco.ToStringLog()}");
                }
            }

            foreach (var tabelaPreco in tabelaPrecos)
            {
                TabelaPreco v = null;
                try
                {
                    v = isFirstLoad ? null : _tabelaPrecoRepositorio.GetSingle(x => x.CampoCodigo == tabelaPreco.CampoCodigo);

                    if (v != null)
                    {
                        v.Descricao = tabelaPreco.Descricao;
                        v.DataAte = tabelaPreco.DataAte;
                        v.DataDe = tabelaPreco.DataDe;
                        v.CodCondicaoPagamento = tabelaPreco.CodCondicaoPagamento;
                        v.RegistroInativo = tabelaPreco.RegistroInativo;
                        v.DataAtualizacao = DateTime.Now;
                        v.UsuarioAtualizacao = UsuarioAtualizacaoServico;
                    }
                    else
                    {
                        tabelaPreco.DataAtualizacao = DateTime.Now;
                        tabelaPreco.UsuarioAtualizacao = UsuarioAtualizacaoServico;
                    }
                }
                catch (Exception e)
                {
                    errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncTabelaPreco)} : {e}. Item: {v.ToStringLog()}");
                }
                try
                {
                    if (v != null)
                    {
                        _tabelaPrecoRepositorio.Update(tabelaPreco);
                    }
                    else
                    {
                        _tabelaPrecoRepositorio.Add(tabelaPreco);
                    }
                }
                catch (Exception e)
                {
                    errosBuilder.AppendLine($"Erro ao persistir Tabela Precos na base de dados. Erro {e}.");
                }
            }
            _escopo.Finalizar();

            if (errosBuilder.Length > 0)
                throw new Exception($"Erros de importação de Tabela Preco: {errosBuilder}.");
        }

        public async Task SyncTabelaPrecoItem(bool isFirstLoad = false)
        {
            var errosBuilder = new StringBuilder();

            var jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.TabelaPreco, TipoTabelaProtheus.DA1);
            do
            {
                var tabelaPrecoItems = new List<TabelaPrecoItem>();

                foreach (var jObj in jArray)
                {
                    var tabelaPrecoItem = new TabelaPrecoItem();

                    try
                    {
                        var tabelaPrecoId = jObj["TabelaPreco_Id"].ToString();
                        TabelaPreco tabelaPreco = null;

                        if (!tabelaPrecoId.IsNullOrEmpty())
                        {
                            tabelaPreco = _tabelaPrecoRepositorio.GetSingle(x => x.CampoCodigo == tabelaPrecoId);
                        }


                        var produtoId = jObj["Produto_Id"].ToString();
                        Produto produto = null;

                        if (!tabelaPrecoId.IsNullOrEmpty() && tabelaPreco != null)
                        {
                            produto = _produtoRepositorio.GetSingle(x => x.CampoCodigo == produtoId);
                        }

                        if (produto != null && tabelaPreco != null)
                        {
                            tabelaPrecoItem.ProdutoId = produto.Id.ToString();
                            tabelaPrecoItem.TabelaPrecoId = tabelaPreco.Id.ToString();
                            tabelaPrecoItem.CampoCodigo = jObj["Id"].ToString();
                            tabelaPrecoItem.PrecoVenda = Convert.ToDecimal(jObj["PrecoVenda"].ToString());
                            tabelaPrecoItem.RegistroInativo = !jObj["Ativo"].ToString().Equals("1");
                            tabelaPrecoItem.DataAtualizacao = DateTime.Now;
                            tabelaPrecoItem.UsuarioAtualizacao = UsuarioAtualizacaoServico;
                            tabelaPrecoItems.Add(tabelaPrecoItem);
                        }
                        else
                        {
                            if (produto == null)
                                errosBuilder.AppendLine($"Produto id {produtoId} não encontrado.");
                            if (tabelaPreco == null)
                                errosBuilder.AppendLine($"TabelaPreco Id {tabelaPrecoId} não encontrado.");
                        }
                    }
                    catch (Exception e)
                    {
                        errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncTabelaPrecoItem)} : {e}. Item: {tabelaPrecoItem.ToStringLog()}");
                    }
                }

                var tabelaPrecoItemRepositorio = new List<TabelaPrecoItem>();

                foreach (var tabelaPreco in tabelaPrecoItems)
                {
                    TabelaPrecoItem v;
                    try
                    {
                        v = isFirstLoad ? null : _tabelaPrecoItemRepositorio.GetSingle(x => x.ProdutoId == tabelaPreco.ProdutoId && x.TabelaPrecoId == tabelaPreco.TabelaPrecoId);

                        if (v != null)
                        {
                            v.ProdutoId = tabelaPreco.ProdutoId;
                            v.TabelaPrecoId = tabelaPreco.TabelaPrecoId;
                            v.CampoCodigo = tabelaPreco.CampoCodigo;
                            v.PrecoVenda = tabelaPreco.PrecoVenda;
                            v.RegistroInativo = tabelaPreco.RegistroInativo;
                            v.DataAtualizacao = DateTime.Now;
                            v.UsuarioAtualizacao = UsuarioAtualizacaoServico;
                        }
                        else
                        {
                            tabelaPreco.DataAtualizacao = DateTime.Now;
                            tabelaPreco.UsuarioAtualizacao = UsuarioAtualizacaoServico;
                            tabelaPrecoItemRepositorio.Add(tabelaPreco);
                        }
                    }
                    catch (Exception e)
                    {
                        errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncTabelaPrecoItem)} : {e}.");
                    }
                }

                try
                {
                    _tabelaPrecoItemRepositorio.AddRange(tabelaPrecoItemRepositorio);
                }
                catch (Exception ex)
                {
                    errosBuilder.AppendLine($"Erro so finalizar o escopo: {nameof(SyncTabelaPrecoItem)} : {ex}.");
                }

                _escopo.Finalizar();
                jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.TabelaPreco, TipoTabelaProtheus.DA1);
            } while (jArray.Count > 0);
            
            if (errosBuilder.Length > 0)
                throw new Exception($"Erros de importação de Tabela Preco Item: {errosBuilder}.");
        }
    }
}