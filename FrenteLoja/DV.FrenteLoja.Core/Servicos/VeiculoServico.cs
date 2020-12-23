using AutoMapper.QueryableExtensions;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Exceptions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace DV.FrenteLoja.Core.Servicos
{
    public class VeiculoServico : IVeiculoServico
    {
        private readonly IRepositorioEscopo _escopo;
        private readonly IRepositorio<Marca> _repositorioMarca;
        private readonly IRepositorio<MarcaModelo> _repositorioMarcaModelo;
        private readonly IRepositorio<MarcaModeloVersao> _respositorioMarcaModeloVersao;
        private readonly IRepositorio<VersaoMotor> _versaoMotorVersao;
        private readonly IRepositorio<Veiculo> _veiculoVersao;
        private readonly IRepositorio<ClienteVeiculo> _repositorioClienteVeiculo;
        private readonly IRepositorio<Cliente> _clienteRepositorio;
        private readonly HttpClient _httpClient;
        public VeiculoServico(IRepositorioEscopo escopo)
        {
            _escopo = escopo;
            _repositorioMarca = escopo.GetRepositorio<Marca>();
            _repositorioMarcaModelo = escopo.GetRepositorio<MarcaModelo>();
            _respositorioMarcaModeloVersao = escopo.GetRepositorio<MarcaModeloVersao>();
            _versaoMotorVersao = escopo.GetRepositorio<VersaoMotor>();
            _veiculoVersao = escopo.GetRepositorio<Veiculo>();
            _repositorioClienteVeiculo = escopo.GetRepositorio<ClienteVeiculo>();
            _clienteRepositorio = escopo.GetRepositorio<Cliente>();
            _httpClient = new HttpClient();
        }
        public IQueryable<Marca> GetMarcas()
        {
            return _repositorioMarca.GetAll();
        }

        public List<MarcaDto> ObterMarcaPorTermo(string termoBusca, int tamanhoPagina, int numeroPagina)
        {
            termoBusca = termoBusca.ToLower();

            return ObterMarcaQuery(termoBusca)
                .Skip(tamanhoPagina * (numeroPagina - 1))
                .Take(tamanhoPagina)
                .ProjectTo<MarcaDto>()
                .ToList();
        }
        /*
        public async Task<ClienteMarcaModeloVersaoDto> ObterVeiculoPorPlaca(string placa)
        {
            /*var marcaModeloVersaoDto =
                   await ObterVeiculoPorPlacaWS(placa)
                ?? ObterVeiculoPorPlacaDBLocal(placa);
            var marcaModeloVersaoDto = ObterVeiculoPorPlacaDBLocal(placa);


            return marcaModeloVersaoDto;
        }
        
        public async Task<ClienteMarcaModeloVersaoDto> ObterVeiculoPorPlacaWS(string placa)
        {
            try
            {
                placa = placa.Contains("-") ? placa.Remove(placa.IndexOf('-'), 1) : placa;
                ClienteMarcaModeloVersaoDto marcaModeloVersaoDto = null;
                var wsAddress = ConfigurationManager.AppSettings["ConsultaPlacaWS"];
                var resourcePath = $"{wsAddress}?placa={placa}";
                _httpClient.Timeout = TimeSpan.FromSeconds(5);
                var response = await _httpClient.GetAsync(resourcePath);
                if (response.IsSuccessStatusCode)
                {
                    marcaModeloVersaoDto = new ClienteMarcaModeloVersaoDto();
                    var content = await response.Content.ReadAsStringAsync();
                    var jObject = JObject.Parse(content);
                    var marcaModeloVersao = jObject["marca"].ToString();
                    var ano = Convert.ToInt32(jObject["anoModelo"].ToString());
                    var marca = marcaModeloVersao.Split('/')[0];
                    var modelo = marcaModeloVersao.Split('/')[1].Split(' ')[0];
                    var index = marcaModeloVersao.Split('/')[1].IndexOf(' ');
                    var versao = marcaModeloVersao.Split('/')[1].Substring(index);

                    var marcaEntidade = _repositorioMarca.GetSingle(x => x.Descricao.Equals(marca,
                        StringComparison.InvariantCultureIgnoreCase));
                    if (marcaEntidade == null)
                    {
                        marcaEntidade = new Marca()
                        {
                            Descricao = marca,
                            DataAtualizacao = DateTime.Now,
                            UsuarioAtualizacao = HttpContext.Current.User.Identity.Name.ToUpper()
                        };
                        _repositorioMarca.Add(marcaEntidade);
                    }

                    var modeloEntidade = _repositorioMarcaModelo.GetSingle(x =>
                        x.Descricao.Equals(modelo, StringComparison.InvariantCultureIgnoreCase));

                    if (modeloEntidade == null)
                    {
                        modeloEntidade = new MarcaModelo()
                        {
                            Descricao = modelo,
                            UsuarioAtualizacao = HttpContext.Current.User.Identity.Name.ToUpper(),
                            DataAtualizacao = DateTime.Now,
                            IdMarca = marcaEntidade.Id
                        };
                        _repositorioMarcaModelo.Add(modeloEntidade);
                    }

                    var versaoEntidade = _respositorioMarcaModeloVersao.GetSingle(x =>
                        x.Descricao.Equals(versao, StringComparison.InvariantCultureIgnoreCase));
                    if (versaoEntidade == null)
                    {
                        versaoEntidade = new MarcaModeloVersao()
                        {
                            Descricao = versao,
                            UsuarioAtualizacao = HttpContext.Current.User.Identity.Name.ToUpper(),
                            DataAtualizacao = DateTime.Now,
                            IdMarcaModelo = modeloEntidade.Id
                        };
                        _respositorioMarcaModeloVersao.Add(versaoEntidade);
                    }
                    var clienteVeiculo = _repositorioClienteVeiculo.GetSingle(x => x.Placa.Equals(placa, StringComparison.InvariantCultureIgnoreCase));

                    marcaModeloVersaoDto.IdMarca = marcaEntidade.Id;
                    marcaModeloVersaoDto.Marca = marcaEntidade.Descricao;
                    marcaModeloVersaoDto.IdModelo = modeloEntidade.Id;
                    marcaModeloVersaoDto.Modelo = modeloEntidade.Descricao;
                    marcaModeloVersaoDto.IdVersao = versaoEntidade.Id;
                    marcaModeloVersaoDto.Versao = versaoEntidade.Descricao;
                    marcaModeloVersaoDto.Ano = ano;
                    marcaModeloVersaoDto.IdCliente = clienteVeiculo?.ClienteId ?? 0;

                }
                return marcaModeloVersaoDto;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task<ClienteMarcaModeloVersaoDto> ObterVeiculoPorPlacaWSAuxiliar(string placa)
        {
            try
            {
                placa = placa.Contains("-") ? placa.Remove(placa.IndexOf('-'), 1).ToUpper() : placa.ToUpper();
                ClienteMarcaModeloVersaoDto marcaModeloVersaoDto = null;
                var wsAddress = ConfigurationManager.AppSettings["ConsultaPlacaWSAuxiliar"];
                var resourcePath = $"{wsAddress}?placa={placa}";
                _httpClient.Timeout = TimeSpan.FromSeconds(5);
                var response = await _httpClient.GetAsync(resourcePath);
                if (response.IsSuccessStatusCode)
                {
                    marcaModeloVersaoDto = new ClienteMarcaModeloVersaoDto();
                    var content = await response.Content.ReadAsStringAsync();
                    var jObject = JObject.Parse(content);
                    var anoArray = jObject["dados_veiculo"]["ano_fabricacao"].ToString().Split('/');
                    var ano = Convert.ToInt32(anoArray.Length == 2 ? anoArray[1] : anoArray[0]);
                    var marca = jObject["dados_veiculo"]["marca"].ToString();
                    var modeloRaw = jObject["dados_veiculo"]["modelo"].ToString();
                    var index = modeloRaw.IndexOf(' ');
                    var modelo = modeloRaw.Substring(0, index);
                    var versao = modeloRaw.Substring(index);


                    var marcaEntidade = _repositorioMarca.GetSingle(x => x.Descricao.Equals(marca,
                        StringComparison.InvariantCultureIgnoreCase));
                    if (marcaEntidade == null)
                    {

                        marcaEntidade = new Marca()
                        {
                            Descricao = marca,
                            DataAtualizacao = DateTime.Now,
                            UsuarioAtualizacao = HttpContext.Current.User.Identity.Name.ToUpper()
                        };
                        _repositorioMarca.Add(marcaEntidade);
                    }

                    var modeloEntidade = _repositorioMarcaModelo.GetSingle(x =>
                        x.Descricao.Equals(modelo, StringComparison.InvariantCultureIgnoreCase));

                    if (modeloEntidade == null)
                    {
                        modeloEntidade = new MarcaModelo()
                        {
                            Descricao = modelo,
                            UsuarioAtualizacao = HttpContext.Current.User.Identity.Name.ToUpper(),
                            DataAtualizacao = DateTime.Now,
                            IdMarca = marcaEntidade.Id
                        };
                        _repositorioMarcaModelo.Add(modeloEntidade);
                    }

                    var versaoEntidade = _respositorioMarcaModeloVersao.GetSingle(x =>
                        x.Descricao.Equals(versao, StringComparison.InvariantCultureIgnoreCase));
                    if (versaoEntidade == null)
                    {
                        versaoEntidade = new MarcaModeloVersao()
                        {
                            Descricao = versao,
                            UsuarioAtualizacao = HttpContext.Current.User.Identity.Name.ToUpper(),
                            DataAtualizacao = DateTime.Now,
                            IdMarcaModelo = modeloEntidade.Id
                        };
                        _respositorioMarcaModeloVersao.Add(versaoEntidade);
                    }

                    var clienteVeiculo = (from clienteVei in _repositorioClienteVeiculo.GetAll()
                                          where clienteVei.Placa == placa
                                          select clienteVei).FirstOrDefault();


                    marcaModeloVersaoDto.IdMarca = marcaEntidade.Id;
                    marcaModeloVersaoDto.Marca = marcaEntidade.Descricao;
                    marcaModeloVersaoDto.IdModelo = modeloEntidade.Id;
                    marcaModeloVersaoDto.Modelo = modeloEntidade.Descricao;
                    marcaModeloVersaoDto.IdVersao = versaoEntidade.Id;
                    marcaModeloVersaoDto.Versao = versaoEntidade.Descricao;
                    marcaModeloVersaoDto.Ano = ano;
                    marcaModeloVersaoDto.IdCliente = clienteVeiculo?.ClienteId ?? 0;

                }
                return marcaModeloVersaoDto;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return null;
            }
        }

        public ClienteMarcaModeloVersaoDto ObterVeiculoPorPlacaDBLocal(string placa)
        {
            try
            {
                placa = placa.Contains("-") ? placa.Remove(placa.IndexOf('-'), 1) : placa;

                var clienteVeiculo = _repositorioClienteVeiculo.GetSingle(x => x.Placa.Equals(placa, StringComparison.InvariantCultureIgnoreCase));

                if (clienteVeiculo == null)
                    throw new NegocioException("Não foi encontrado veículo para a placa informada.");

                ClienteMarcaModeloVersaoDto marcaModeloVersaoDto = new ClienteMarcaModeloVersaoDto()
                {
                    IdMarca = clienteVeiculo.Veiculo.MarcaModeloVersao.MarcaModelo.Marca.Id,
                    Marca = clienteVeiculo.Veiculo.MarcaModeloVersao.MarcaModelo.Marca.Descricao,
                    IdModelo = clienteVeiculo.Veiculo.MarcaModeloVersao.MarcaModelo.Id,
                    Modelo = clienteVeiculo.Veiculo.MarcaModeloVersao.MarcaModelo.Descricao,
                    IdVersao = clienteVeiculo.Veiculo.MarcaModeloVersao.Id,
                    Versao = clienteVeiculo.Veiculo.MarcaModeloVersao.Descricao,
                    IdCliente = clienteVeiculo.ClienteId,
                    VersaoMotor = clienteVeiculo.Veiculo.VersaoMotor.Descricao,
                    IdVersaoMotor = clienteVeiculo.Veiculo.IdVersaoMotor,
                    Ano = clienteVeiculo.VeiculoAno
                };
                return marcaModeloVersaoDto;
            }
            catch (NegocioException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        */
        public int QuantidadeMarcasPorTermo(string termoBusca)
        {
            return ObterMarcaQuery(termoBusca).Count();
        }

        private IQueryable<Marca> ObterMarcaQuery(string termoBusca)
        {
            termoBusca = string.Format("{0}%", termoBusca).ToLower();
            return from marca in _repositorioMarca.GetAll()
                   where DbFunctions.Like(marca.Descricao, termoBusca)
                   && !marca.RegistroInativo
                   orderby marca.Descricao
                   select marca;
        }

        public List<MarcaModeloDto> ObterModelosPeloIdMarca(string termoBusca, int tamanhoPagina, int numeroPagina, long idMarca)
        {
            return ObterModelosPeloIdMarcaQuery(termoBusca, idMarca)
                .Skip(tamanhoPagina * (numeroPagina - 1))
                .Take(tamanhoPagina)
                .ProjectTo<MarcaModeloDto>()
                .ToList();
        }

        public List<ClienteMarcaModeloVersaoDto> ObterVersoesPeloIdModelo(string termoBusca, int tamanhoPagina, int numeroPagina, long idMarca)
        {
            return ObterVersoesPeloIdModeloQuery(termoBusca, idMarca)
                .Skip(tamanhoPagina * (numeroPagina - 1))
                .Take(tamanhoPagina)
                .ProjectTo<ClienteMarcaModeloVersaoDto>()
                .ToList();
        }


        public Cliente ObterClientePorPlaca(string placa)
        {

            var clienteVeiculo = _repositorioClienteVeiculo.Get(x => x.Placa.Equals(placa, StringComparison.InvariantCultureIgnoreCase)).OrderByDescending(x => x.DataAtualizacao).FirstOrDefault();

            return clienteVeiculo != null
                ? _clienteRepositorio.GetSingle(x =>
                    x.IdCliente == clienteVeiculo.ClienteId) : null;
        }

        public int TamanhoModelosPorTermo(string termoBusca, long idMarca)
        {
            return ObterModelosPeloIdMarcaQuery(termoBusca, idMarca).Count();
        }

        public int TamanhoVersaoPorTermo(string termoBusca, long idMarca)
        {
            return ObterVersoesPeloIdModeloQuery(termoBusca, idMarca).Count();
        }

        private IQueryable<MarcaModeloVersao> ObterVersoesPeloIdModeloQuery(string termoBusca, long idModelo)
        {
            if (string.IsNullOrEmpty(termoBusca))
            {
                return from marcaModeloVersao in _respositorioMarcaModeloVersao.GetAll()
                       where marcaModeloVersao.MarcaModelo.Id == idModelo && !marcaModeloVersao.RegistroInativo
                       orderby marcaModeloVersao.Descricao
                       select marcaModeloVersao;
            }
            else
            {
                termoBusca = string.Format("{0}%", termoBusca).ToLower();
                return from marcaModeloVersao in _respositorioMarcaModeloVersao.GetAll()
                       where marcaModeloVersao.MarcaModelo.Id == idModelo && DbFunctions.Like(marcaModeloVersao.Descricao, termoBusca)
                              && !marcaModeloVersao.RegistroInativo
                       orderby marcaModeloVersao.Id
                       select marcaModeloVersao;
            }

        }

        public int TamanhoVersaoMotorPorTermo(string termoBusca, long idVersao)
        {
            return ObterVersoesMotorPeloIdVersaoVeiculoQuery(termoBusca, idVersao).Count();
        }

        public List<VersaoMotorDTO> ObterVersoesMotorPeloIdVersaoVeiculo(string termoBusca, long idVersao)
        {
            return ObterVersoesMotorPeloIdVersaoVeiculoQuery(termoBusca, idVersao)
                .ProjectTo<VersaoMotorDTO>()
                .ToList();
        }

        private IQueryable<VersaoMotor> ObterVersoesMotorPeloIdVersaoVeiculoQuery(string termoBusca, long idVersao)
        {
            if (string.IsNullOrEmpty(termoBusca))
            {
                IQueryable<Veiculo> v = from veiculo in _veiculoVersao.GetAll()
                                        where veiculo.IdMarcaModeloVersao == idVersao
                                        select veiculo;
                return from versaoMotor in _versaoMotorVersao.GetAll()
                       where v.All(a => a.IdVersaoMotor == versaoMotor.Id)
                       orderby versaoMotor.Descricao
                       select versaoMotor;
            }
            else
            {
                IQueryable<Veiculo> v = from Veiculo in _veiculoVersao.GetAll()
                                        where Veiculo.IdMarcaModeloVersao == idVersao
                                        select Veiculo;
                termoBusca = string.Format("{0}%", termoBusca).ToLower();
                return from versaoMotor in _versaoMotorVersao.GetAll()
                       where v.All(a => a.IdVersaoMotor == versaoMotor.Id) && DbFunctions.Like(versaoMotor.Descricao, termoBusca)
                       orderby versaoMotor.Id
                       select versaoMotor;
            }

        }

        private IQueryable<MarcaModelo> ObterModelosPeloIdMarcaQuery(string termoBusca, long idMarca)
        {
            if (string.IsNullOrEmpty(termoBusca))
            {
                return from marcaModelo in _repositorioMarcaModelo.GetAll()
                       where marcaModelo.Marca.Id == idMarca &&
                       !marcaModelo.RegistroInativo
                       orderby marcaModelo.Descricao
                       select marcaModelo;
            }
            else
            {
                termoBusca = string.Format("{0}%", termoBusca).ToLower();
                return from marcaModelo in _repositorioMarcaModelo.GetAll()
                       where marcaModelo.Marca.Id == idMarca && DbFunctions.Like(marcaModelo.Descricao, termoBusca)
                       && !marcaModelo.RegistroInativo
                       orderby marcaModelo.Id
                       select marcaModelo;
            }

        }
        /// <summary>
        /// Quando orçamento do tipo retira e não informar o veiculo, será a marca 058 e modelo 001071
        /// </summary>
        /// <returns></returns>
        public ClienteMarcaModeloVersaoDto ObterVeiculoRetira()
        {
            var veiculoRetira = new ClienteMarcaModeloVersaoDto();
            var marcaModeloVersao = _respositorioMarcaModeloVersao.Get(a => a.MarcaModelo.Marca.CampoCodigo == "058" && a.MarcaModelo.CampoCodigo == "001071").FirstOrDefault();

            if (marcaModeloVersao != null)
            {
                veiculoRetira.IdMarca = marcaModeloVersao.MarcaModelo.IdMarca;
                veiculoRetira.IdModelo = marcaModeloVersao.IdMarcaModelo;
                veiculoRetira.IdVersao = marcaModeloVersao.Id;
                veiculoRetira.Marca = marcaModeloVersao.MarcaModelo.Marca.Descricao;
                veiculoRetira.Modelo = marcaModeloVersao.MarcaModelo.Descricao;
                veiculoRetira.Versao = marcaModeloVersao.Descricao;

            }

            return veiculoRetira;
        }
    }
}
