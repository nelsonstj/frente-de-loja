using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Extensions;
using DV.FrenteLoja.Core.Util;

namespace DV.FrenteLoja.Core.Servicos
{
    public class CargaConvenioServico : ICargaConvenioServico
    {
        private readonly IRepositorioEscopo _escopo;
        private readonly ICargaCadastrosProtheusSyncApi _protheusSyncApi;
        private readonly IRepositorio<Cliente> _clienteRepositorio;
        private readonly IRepositorio<TabelaPreco> _tabelaPrecoRepositorio;
        //private readonly IRepositorio<Convenio> _convenioRepositorio;
        private readonly IRepositorio<Produto> _produtoRepositorio;
        private readonly IRepositorio<GrupoProduto> _grupoProdutoRepositorio;
        //private readonly IRepositorio<ConvenioCliente> _convenioClienteRepositorio;
        private readonly IRepositorio<ConvenioProduto> _convenioProdutoRepositorio;
        private readonly IRepositorio<CondicaoPagamento> _condicaoPagamentoRepositorio;
        private readonly IRepositorio<ConvenioCondicaoPagamento> _convenioCondicaoPagamentoRepositorio;
        private const string UsuarioAtualizacaoServico = "Serviço de Atualização";


        public CargaConvenioServico(IRepositorioEscopo escopo, ICargaCadastrosProtheusSyncApi protheusSyncApi)
        {
            _escopo = escopo;
            _protheusSyncApi = protheusSyncApi;
            _clienteRepositorio = escopo.GetRepositorio<Cliente>();
            _tabelaPrecoRepositorio = escopo.GetRepositorio<TabelaPreco>();
            //_convenioRepositorio = escopo.GetRepositorio<Convenio>();
            _condicaoPagamentoRepositorio = _escopo.GetRepositorio<CondicaoPagamento>();
            _convenioCondicaoPagamentoRepositorio = _escopo.GetRepositorio<ConvenioCondicaoPagamento>();
            //_convenioClienteRepositorio = _escopo.GetRepositorio<ConvenioCliente>();
            _convenioProdutoRepositorio = _escopo.GetRepositorio<ConvenioProduto>();
            _produtoRepositorio = _escopo.GetRepositorio<Produto>();
            _grupoProdutoRepositorio = _escopo.GetRepositorio<GrupoProduto>();
        }

        public async Task SyncCondicaoPagamento(bool isFirstLoad = false)
        {
            var errosBuilder = new StringBuilder();
            var jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Consulta, TipoTabelaProtheus.SE4);
           
            do
            {
                var condicaoPagamentoList = new List<CondicaoPagamento>();
                foreach (var jObj in jArray)
                {
                    var condicaoPagamento = new CondicaoPagamento();
                    try
                    {
                        condicaoPagamento.Descricao = jObj["Descricao"].ToString();
                        condicaoPagamento.FormaCondicaoPagamento = jObj["CondicaoPagamento"].ToString();
                        condicaoPagamento.FormaPagamento = jObj["FormaPagamento"].ToString();
                        condicaoPagamento.CampoCodigo = jObj["Id"].ToString();
                        condicaoPagamento.QtdParcelas = condicaoPagamento.FormaCondicaoPagamento.Split(',').Count();
                        condicaoPagamento.ValorAcrescimo = Convert.ToDecimal(jObj["ValorAcrescimo"].ToString());
                        condicaoPagamento.ListaTipoVenda = jObj["ListaTipoVenda"].ToString();
                        condicaoPagamento.RegistroInativo = !Convert.ToBoolean(jObj["Ativo"].ToString());
                        condicaoPagamento.TipoCondicaoPagamento = jObj["TipoCondicaoPagamento"].ToString();
                        condicaoPagamento.CondicaoDeVenda = Convert.ToBoolean(jObj["CondicaoDeVenda"].ToString());

                        if (condicaoPagamento.QtdParcelas == 0)
                        {
                            condicaoPagamento.QtdParcelas = 1;
                        }

                        if (condicaoPagamento.TipoCondicaoPagamento == "5")
                        {
                            condicaoPagamento.QtdParcelas = Convert.ToInt32(condicaoPagamento.FormaCondicaoPagamento.Split(',')[1]);
                        }

                        condicaoPagamentoList.Add(condicaoPagamento);
                    }
                    catch (Exception e)
                    {
                        errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncCondicaoPagamento)} : {e}. Item: {condicaoPagamento.ToStringLog()}");
                    }
                }

                var condicaoPagamentoRepositorio = new List<CondicaoPagamento>();

                foreach (var condicaoPagamento in condicaoPagamentoList)
                {
                    CondicaoPagamento condPagto = null;
                    try
                    {
                        condPagto = isFirstLoad ? null : _condicaoPagamentoRepositorio.GetSingle(x => x.CampoCodigo == condicaoPagamento.CampoCodigo);

                        if (condPagto != null)
                        {
                            condPagto.Descricao = condicaoPagamento.Descricao;
                            condPagto.FormaCondicaoPagamento = condicaoPagamento.FormaCondicaoPagamento;
                            condPagto.QtdParcelas = condicaoPagamento.QtdParcelas;
                            condPagto.ValorAcrescimo = condicaoPagamento.ValorAcrescimo;
                            condPagto.FormaPagamento = condicaoPagamento.FormaPagamento;
                            condPagto.RegistroInativo = condicaoPagamento.RegistroInativo;
                            condPagto.ListaTipoVenda = condicaoPagamento.ListaTipoVenda;
                            condPagto.DataAtualizacao = DateTime.Now;
                            condPagto.CondicaoDeVenda = condicaoPagamento.CondicaoDeVenda;
                            condPagto.UsuarioAtualizacao = UsuarioAtualizacaoServico;
                        }
                        else
                        {
                            condicaoPagamento.DataAtualizacao = DateTime.Now;
                            condicaoPagamento.UsuarioAtualizacao = UsuarioAtualizacaoServico;
                            condicaoPagamentoRepositorio.Add(condicaoPagamento);
                        }
                    }
                    catch (Exception e)
                    {
                        errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncCondicaoPagamento)} : {e}. Item: {condPagto.ToStringLog()}");
                    }                    
                }

                try
                {
                    _condicaoPagamentoRepositorio.AddRange(condicaoPagamentoRepositorio);
                }
                catch (Exception e)
                {
                    errosBuilder.AppendLine($"Erro ao persistir Condições de Pagamentos na base de dados. Erro {e}.");
                }

                _escopo.Finalizar();
                jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Consulta, TipoTabelaProtheus.SE4);
            } while (jArray.Count > 0);


            if (errosBuilder.Length > 0)
                throw new Exception($"Erros de importação de Condicao Pagamento: {errosBuilder}.");
        }

        public async Task SyncConvenioCondicaoPagamento(bool isFirstLoad = false)
        {
            var errosBuilder = new StringBuilder();
            var jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Convenio, TipoTabelaProtheus.PBQ);

       
            do
            {
                var convenioCondicaoPagamentoList = new List<ConvenioCondicaoPagamento>();
                foreach (var jObj in jArray)
                {
                    ConvenioCondicaoPagamento convenioCondicaoPagamento = new ConvenioCondicaoPagamento();
                    try
                    {
                        var convenioId = jObj["Convenio_Id"].ToString();
                        var convenio = new ConvenioDto() ;// _convenioRepositorio.GetSingle(x => x.CampoCodigo == convenioId);

                        var condicaoPagamentoId = jObj["CondicaoPagamento_Id"].ToString();
                        var condicaoPagamento = _condicaoPagamentoRepositorio.GetSingle(x => x.CampoCodigo == condicaoPagamentoId);


                        if (convenio != null && condicaoPagamento != null)
                        {
                            convenioCondicaoPagamento.IdCondicaoPagamento = condicaoPagamento.Id.ToString();
                            convenioCondicaoPagamento.IdConvenio = convenio.IdConvenio;
                            convenioCondicaoPagamento.CampoCodigo = jObj["Id"].ToString();
                            convenioCondicaoPagamento.RegistroInativo = !Convert.ToBoolean(jObj["Ativo"].ToString());
                            convenioCondicaoPagamentoList.Add(convenioCondicaoPagamento);

                        }
                        else
                        {
                            if (convenio == null)
                                errosBuilder.AppendLine($"Convenio Id {convenioId} não encontrado.");

                            if (condicaoPagamento == null)
                                errosBuilder.AppendLine($"CondicaoPagamento Id {condicaoPagamentoId} não encontrado.");
                        }

                    }
                    catch (Exception e)
                    {
                        errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncConvenioCondicaoPagamento)} : {e}. Item: {convenioCondicaoPagamento.ToStringLog()}");
                    }
                }

                var convenioCondicaoPagamentoRepositorio = new List<ConvenioCondicaoPagamento>();

                foreach (ConvenioCondicaoPagamento convenioCondicaoPagamento in convenioCondicaoPagamentoList)
                {
                    var v = isFirstLoad ? null : _convenioCondicaoPagamentoRepositorio.GetSingle(x => x.CampoCodigo == convenioCondicaoPagamento.CampoCodigo);

                    try
                    {
                        if (v != null)
                        {
                            v.RegistroInativo = convenioCondicaoPagamento.RegistroInativo;
                            v.DataAtualizacao = DateTime.Now;
                            v.UsuarioAtualizacao = UsuarioAtualizacaoServico;
                        }
                        else
                        {
                            convenioCondicaoPagamento.DataAtualizacao = DateTime.Now;
                            convenioCondicaoPagamento.UsuarioAtualizacao = UsuarioAtualizacaoServico;
                            convenioCondicaoPagamentoRepositorio.Add(convenioCondicaoPagamento);
                        }
                    }
                    catch (Exception e)
                    {
                        errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncConvenioCondicaoPagamento)} : {e}. Item: {v.ToStringLog()}");
                    }                    
                }

                try
                {
                    _convenioCondicaoPagamentoRepositorio.AddRange(convenioCondicaoPagamentoRepositorio);
                }
                catch (Exception e)
                {
                    errosBuilder.AppendLine($"Erro ao persistir Convenio Condição Pagamento na base de dados. Erro {e}.");
                }

                _escopo.Finalizar();
                jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Convenio, TipoTabelaProtheus.PBQ);
            } while (jArray.Count > 0);


            if (errosBuilder.Length > 0)
                throw new Exception($"Erros de importação de Convenio Condição Pagamento: {errosBuilder}.");
        }

        public async Task SyncConvenio(bool isFirstLoad = false)
        {
            var jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Convenio, TipoTabelaProtheus.PA6);

            StringBuilder errosBuilder = new StringBuilder();
            var convenios = new List<Convenio>();

            foreach (var jObj in jArray)
            {
                var convenio = new Convenio();

                try
                {
                    var clienteId = jObj["Cliente_Id"].ToString();
                    var clienteLoja = jObj["Cliente_Loja"].ToString();

                    Cliente cliente = null;
                    if (!clienteId.IsNullOrEmpty() && !clienteLoja.IsNullOrEmpty())
                    {
                        cliente = _clienteRepositorio.GetSingle(x => x.CampoCodigo == clienteId && x.Loja == clienteLoja);
                    }


                    var tabelaPrecoId = jObj["TabelaPreco_Id"].ToString();

                    TabelaPreco tabelaPreco = null;

                    if (!tabelaPrecoId.IsNullOrEmpty())
                    {
                        tabelaPreco = _tabelaPrecoRepositorio.GetSingle(x => x.CampoCodigo == tabelaPrecoId);
                    }



                    if (tabelaPreco != null)
                    {
                        convenio.IdCliente = cliente.IdCliente;
                        convenio.IdTabelaPreco = tabelaPreco.IdTabelaPreco;

                        convenio.Descricao = jObj["Descricao"].ToString();
                        convenio.Observacoes = jObj["Observacoes"].ToString();
                        convenio.DataInicioVigencia = ProtheusConversions.ProtheusDate2DotNetDate(jObj["DataInicioVigencia"].ToString());
                        convenio.DataFimVigencia = ProtheusConversions.ProtheusDate2DotNetDate(jObj["DataFimVigencia"].ToString());
                        convenio.TrocaCliente = jObj["TrocaCliente"].ToString().ToLower() == "s";
                        convenio.CampoCodigo = jObj["Id"].ToString();
                        convenio.TrocaPreco = (TrocaPrecoConvenio)Convert.ToInt32(jObj["TrocaPreco"].ToString());
                        convenio.TrocaProduto = jObj["TrocaProduto"].ToString().ToLower() == "s";
                        convenio.TrocaTabelaPreco = jObj["TrocaTabelaPreco"].ToString().ToLower() == "s";
                        //convenio.RegistroInativo = !Convert.ToBoolean(jObj["Ativo"].ToString());
                        convenios.Add(convenio);
                    }
                    else
                    {
                        errosBuilder.AppendLine($"TabelaPreco Id {tabelaPrecoId} não encontrado.");
                    }
                }
                catch (Exception e)
                {
                    errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncConvenio)} : {e}. Item: {convenio.ToStringLog()}");
                }
            }


            List<Convenio> convenioRepositorio = new List<Convenio>();
            foreach (var convenio in convenios)
            {
                Convenio v = null;
                try
                {
                    v = isFirstLoad ? null : new Convenio();//_convenioRepositorio.GetSingle(x => x.CampoCodigo == convenio.CampoCodigo);

                    if (v != null)
                    {
                        v.Descricao = convenio.Descricao;
                        //v.RegistroInativo = convenio.RegistroInativo;
                        v.Observacoes = convenio.Observacoes;
                        v.IdTabelaPreco = convenio.IdTabelaPreco;
                        v.DataInicioVigencia = convenio.DataInicioVigencia;
                        v.DataFimVigencia = convenio.DataFimVigencia;
                        v.TrocaCliente = convenio.TrocaCliente;
                        v.TrocaPreco = convenio.TrocaPreco;
                        v.TrocaProduto = convenio.TrocaProduto;
                        v.IdCliente = convenio.IdCliente;
                        v.TrocaTabelaPreco = convenio.TrocaTabelaPreco;
                        //v.DataAtualizacao = DateTime.Now;
                        //v.UsuarioAtualizacao = UsuarioAtualizacaoServico;
                    }
                    else
                    {
                        //convenio.DataAtualizacao = DateTime.Now;
                        //convenio.UsuarioAtualizacao = UsuarioAtualizacaoServico;
                        convenioRepositorio.Add(convenio);
                    }                    
                }
                catch (Exception e)
                {
                    errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncConvenio)} : {e}. Item: {v.ToStringLog()}");
                }
            }

            try
            {
                //_convenioRepositorio.AddRange(convenioRepositorio);
                _escopo.Finalizar();
            }
            catch (Exception e)
            {
                errosBuilder.AppendLine($"Erro ao persistir Convenios na base de dados. Erro {e}.");
            }


            if (errosBuilder.Length > 0)
                throw new Exception($"Erros de importação de Convenios: {errosBuilder}.");
        }

        public async Task SyncConvenioCliente(bool isFirstLoad = false)
        {
            var jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Convenio, TipoTabelaProtheus.PBO);

            var errosBuilder = new StringBuilder();
        
            do
            {
                /*
                var convenioClientes = new List<ConvenioCliente>();

                foreach (var jObj in jArray)
                {
                    var convenioCliente = new ConvenioCliente();
                    try
                    {
                        var clienteId = jObj["Cliente_Id"].ToString();
                        var clienteLoja = jObj["Cliente_Loja"].ToString();

                        Cliente cliente = null;
                        if (!clienteId.IsNullOrEmpty() && !clienteLoja.IsNullOrEmpty())
                        {
                            cliente = _clienteRepositorio.GetSingle(x => x.CampoCodigo == clienteId && x.Loja == clienteLoja);
                        }



                        var convenioId = jObj["Convenio_Id"].ToString();

                        Convenio convenio = null;
                        if (!convenioId.IsNullOrEmpty() && cliente != null)
                        {
                            convenio = new Convenio();// _convenioRepositorio.GetSingle(x => x.CampoCodigo == convenioId);
                        }

                        if (cliente != null && convenio != null)
                        {
                            convenioCliente.IdCliente = cliente.IdCliente;
                            convenioCliente.IdConvenio = convenio.IdConvenio;
                            //convenioCliente.CampoCodigo = jObj["Id"].ToString();
                            //convenioCliente.RegistroInativo = !Convert.ToBoolean(jObj["Ativo"].ToString());
                            convenioClientes.Add(convenioCliente);
                        }
                        else
                        {
                            if (cliente == null)
                                errosBuilder.AppendLine($"Cliente/Loja id {clienteId}/{clienteLoja} não encontrado.");
                            if (convenio == null)
                                errosBuilder.AppendLine($"Convenio Id {convenioId} não encontrado.");
                        }
                    }
                    catch (Exception e)
                    {
                        errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncConvenioCliente)} : {e}. Item: {convenioCliente.ToStringLog()}");
                    }
                }

                var convenioClienteRepositorio = new List<ConvenioCliente>();

                foreach (var convenio in convenioClientes)
                {
                    ConvenioCliente v = null;
                    try
                    {
                        //v = isFirstLoad ? null : _convenioClienteRepositorio.GetSingle(x => x.CampoCodigo == convenio.CampoCodigo);

                        if (v != null)
                        {
                            //v.RegistroInativo = convenio.RegistroInativo;
                            //v.DataAtualizacao = DateTime.Now;
                            //v.UsuarioAtualizacao = UsuarioAtualizacaoServico;
                        }
                        else
                        {
                            //convenio.DataAtualizacao = DateTime.Now;
                            //convenio.UsuarioAtualizacao = UsuarioAtualizacaoServico;
                            convenioClienteRepositorio.Add(convenio);
                        }                        
                    }
                    catch (Exception e)
                    {
                        errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncConvenioCliente)} : {e}. Item: {v.ToStringLog()}");
                    }
                }

                try
                {
                    //_convenioClienteRepositorio.AddRange(convenioClienteRepositorio);
                }
                catch (Exception e)
                {
                    errosBuilder.AppendLine($"Erro ao persistir Convenios de Clientes na base de dados. Erro {e}.");
                }
                */
                _escopo.Finalizar();
                jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Convenio, TipoTabelaProtheus.PBO);
            } while (jArray.Count > 0);
            
            if (errosBuilder.Length > 0)
                throw new Exception($"Erros de importação de Convenio Cliente: {errosBuilder}.");
        }

        public async Task SyncConvenioProduto(bool isFirstLoad = false)
        {
            var jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Convenio, TipoTabelaProtheus.PBP);
          
            var errosBuilder = new StringBuilder();
            do
            {

                var convenioProdutos = new List<ConvenioProduto>();

                foreach (var jObj in jArray)
                {
                    var convenioProduto = new ConvenioProduto();
                    try
                    {
                        var produtoId = jObj["Produto_id"].ToString();

                        Produto produto = null;

                        if (!produtoId.IsNullOrEmpty())
                        {
                            produto = _produtoRepositorio.GetSingle(x => x.CampoCodigo == produtoId);
                        }



                        var grupoProdutoId = jObj["GrupoProduto_id"].ToString();
                        var grupoProduto = _grupoProdutoRepositorio.GetSingle(x => x.CampoCodigo == grupoProdutoId);

                        var convenioId = jObj["Convenio_Id"].ToString();
                        var convenio = new Convenio(); // _convenioRepositorio.GetSingle(x => x.CampoCodigo == convenioId);


                        if (convenio != null)
                        {
                            convenioProduto.IdProduto = produto.IdProduto;
                            convenioProduto.IdGrupoProduto = grupoProduto.IdGrupoProduto;
                            convenioProduto.IdConvenio = convenio.IdConvenio;
                            convenioProduto.CampoCodigo = jObj["Id"].ToString();
                            convenioProduto.RegistroInativo = !Convert.ToBoolean(jObj["Ativo"].ToString());
                            convenioProduto.TipoPreco = (TipoPreco)Convert.ToInt32(jObj["TipoPreco"].ToString());
                            convenioProdutos.Add(convenioProduto);
                        }
                        else
                        {

                            errosBuilder.AppendLine($"Convenio id {convenioId} não encontrado.");
                        }
                    }
                    catch (Exception e)
                    {
                        errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncConvenioProduto)} : {e}. Item: {convenioProduto.ToStringLog()}");
                    }
                }

                var convenioProdutoRepositorio = new List<ConvenioProduto>();

                foreach (var convenio in convenioProdutos)
                {
                    ConvenioProduto v = null;
                    try
                    {
                        v = isFirstLoad ? null : _convenioProdutoRepositorio.GetSingle(x => x.CampoCodigo == convenio.CampoCodigo);

                        if (v != null)
                        {
                            v.TipoPreco = convenio.TipoPreco;
                            v.RegistroInativo = convenio.RegistroInativo;
                            v.DataAtualizacao = DateTime.Now;
                            v.UsuarioAtualizacao = UsuarioAtualizacaoServico;
                        }
                        else
                        {
                            convenio.DataAtualizacao = DateTime.Now;
                            convenio.UsuarioAtualizacao = UsuarioAtualizacaoServico;
                            convenioProdutoRepositorio.Add(convenio);
                        }                        
                    }
                    catch (Exception e)
                    {
                        errosBuilder.AppendLine($"Erro no metodo: {nameof(SyncConvenioProduto)} : {e}. Item: {v.ToStringLog()}");
                    }
                }

                try
                {
                    _convenioProdutoRepositorio.AddRange(convenioProdutoRepositorio);
                }
                catch (Exception e)
                {
                    errosBuilder.AppendLine($"Erro ao persistir Convenios de Produtos na base de dados. Erro {e}.");
                }

                _escopo.Finalizar();
                jArray = await _protheusSyncApi.GetDataFromKeyTable(EndpointProtheus.Convenio, TipoTabelaProtheus.PBP);
            } while (jArray.Count > 0);
            

            if (errosBuilder.Length > 0)
                throw new Exception($"Erros de importação de Convenio Produto: {errosBuilder}.");
        }
    }
}