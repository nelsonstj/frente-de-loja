using System;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Grafo;
using DV.FrenteLoja.Core.Infra.EntityFramework;
using DV.FrenteLoja.Core.ProtheusAPI;
using DV.FrenteLoja.Core.Servicos;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System.Configuration;
using System.Diagnostics;
using Container = SimpleInjector.Container;

namespace DV.FrenteLoja.Service
{
    public class BaseConfig
    {
        public static readonly Container Container = new Container();
        public static ConfigGrafo ContainerGrafo;
        public static IRepositorioEscopo RepositorioEscopo;


        public static void ConfigurarDependencias()
        {

            // contexto dos repositorios.
	        var dellaviaContexto = new DellaviaContexto();
            RepositorioEscopo = new DellaviaEscopo(dellaviaContexto);
            ContainerGrafo = new ConfigGrafo(ConfigurationManager.ConnectionStrings["DellaviaGrafoContexto"].ConnectionString);
            ContainerGrafo.Conectar(Core.Grafo.Enumerator.TipoConexao.Sincrona);

            Container.Options.DefaultScopedLifestyle = new ThreadScopedLifestyle();

			
			Container.Register<DellaviaContexto>(Lifestyle.Scoped);
			Container.Register<IRepositorioEscopo, DellaviaEscopo>(Lifestyle.Scoped);
			Container.Register<ILogIntegracaoServico, LogIntegracaoServico>(Lifestyle.Scoped);
			Container.Register<IOrcamentoServico, OrcamentoServico>(Lifestyle.Scoped);
			Container.Register<IVendedorServico, VendedorServico>(Lifestyle.Scoped);
			Container.Register<ILogCargaCatalogoServico, LogCargaCatalogoServico>(Lifestyle.Scoped);
			Container.Register<ICatalogoServico>(() => new CatalogoServico(RepositorioEscopo, ContainerGrafo.contextoGrafo, new Core.Servicos.GrupoProdutoServico(RepositorioEscopo)), Lifestyle.Scoped);
			Container.Register<ITabelaPrecoServico, TabelaPrecoServico>(Lifestyle.Scoped);
			Container.Register<IVeiculoServico, VeiculoServico>(Lifestyle.Scoped);
			Container.Register<ITipoVendaServico, TipoVendaServico>(Lifestyle.Scoped);
			Container.Register<IConvenioServico, ConvenioServico>(Lifestyle.Scoped);
			Container.Register<IClienteServico, ClienteServico>(Lifestyle.Scoped);
			Container.Register<IParametroGeralServico, ParametroGeralServico>(Lifestyle.Scoped);
			Container.Register<ILojaDellaViaServico, LojaDellaViaServico>(Lifestyle.Scoped);
			Container.Register<ICatalogoProtheusApi, CatalogoApi>(Lifestyle.Scoped);
			Container.Register<ILoginProtheusApi, LoginApi>(Lifestyle.Scoped);
			Container.Register<IEstoqueProtheusApi, EstoqueApi>(Lifestyle.Scoped);
			Container.Register<IOrcamentoItemServico, OrcamentoItemServico>(Lifestyle.Scoped);
			Container.Register<IProdutoServico, ProdutoServico>(Lifestyle.Scoped);
			Container.Register<ITransportadoraServico, TransportadoraServico>(Lifestyle.Scoped);
			Container.Register<IOrcamentoApi, OrcamentoApi>(Lifestyle.Scoped);
			Container.Register<ICargaProdutoServico>(() => new Core.Servicos.CargaProdutoServico(RepositorioEscopo, new CargaCadastrosSyncApi(), ContainerGrafo));
			Container.Register<ICargaCadastrosBasicosService>(() => new Core.Servicos.CargaCadastrosBasicosService(RepositorioEscopo, new CargaCadastrosSyncApi()));
			Container.Register<ICargaDescontosServico>(() => new Core.Servicos.CargaDescontosServico(RepositorioEscopo, new CargaCadastrosSyncApi()));
			Container.Register<ICargaClienteServico>(() => new Core.Servicos.CargaClienteServico(RepositorioEscopo, new CargaCadastrosSyncApi()));
			Container.Register<ICargaConvenioServico>(() => new Core.Servicos.CargaConvenioServico(RepositorioEscopo, new CargaCadastrosSyncApi()));
			Container.Register<ICargaPrecoServico>(() => new Core.Servicos.CargaPrecoServico(RepositorioEscopo, new CargaCadastrosSyncApi()));
			Container.Register<ICargaOrcamentoServico>(() => new Core.Servicos.CargaOrcamentoServico(RepositorioEscopo, new OrcamentoApi(RepositorioEscopo, dellaviaContexto, new CalculoImpostosApi(RepositorioEscopo)), new SolicitacaoAnaliseCreditoApi()));
			Container.Register<ICondicaoPagamentoParcelasApi, CondicaoPagamentoParcelasApi>(Lifestyle.Scoped);
            Container.Register<ICargaVeiculoServico>(() => new Core.Servicos.CargaVeiculoServico(RepositorioEscopo, dellaviaContexto));
            Container.Register<ICargaVeiculoProdutosServico>(() => new Core.Servicos.CargaVeiculosProdutoServico(RepositorioEscopo, dellaviaContexto));
			Container.Register<ICheckupCarServico<CheckupCarDto>, CheckupCarServico<CheckupCarDto>>(Lifestyle.Scoped);
			Container.Register<ICheckupTruckServico<CheckupTruckDto>, CheckupTruckServico<CheckupTruckDto>>(Lifestyle.Scoped);
			Container.Register<ICheckupServico<CheckupDto>, CheckupServico>(Lifestyle.Scoped);
			Container.Register<ICalculoImpostosApi, CalculoImpostosApi>(Lifestyle.Scoped);
			Container.Register<IHomeServico, HomeServico>(Lifestyle.Scoped);
			Container.Register<IImpostosServico, ImpostosServico>(Lifestyle.Scoped);

            //var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            //Container.RegisterCollection(typeof(IValidator), assemblies);

            //adição de serviços
            //AutoMapperConfig.RegisterMappings(container);
	        try
	        {
		        Container.Verify();
	        }
	        catch (Exception e)
	        {
		        Debug.WriteLine(e);
	        }
        }
    }
}