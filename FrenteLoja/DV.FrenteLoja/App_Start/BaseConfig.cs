//using DB.SQLServer.Config;
//using DB.SQLServer.Infraestrutura;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Grafo;
using DV.FrenteLoja.Core.Infra.EntityFramework;
using DV.FrenteLoja.Core.ProtheusAPI;
using DV.FrenteLoja.Core.Servicos;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace DV.FrenteLoja
{
	public class BaseConfig
	{
		public static readonly Container Container = new Container();
		public static ConfigGrafo ContainerGrafo;
		public static void ConfigurarDependencias()
		{

			//grafo
			ContainerGrafo = new ConfigGrafo(ConfigurationManager.ConnectionStrings["DellaviaGrafoContexto"].ConnectionString);
			ContainerGrafo.Conectar(Core.Grafo.Enumerator.TipoConexao.Sincrona);
			var dellaviaContexto = new DellaviaContexto();
            IRepositorioEscopo escopo = new DellaviaEscopo(dellaviaContexto);

            //contexto
            Container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();


			Container.Register<DellaviaContexto>(Lifestyle.Scoped);
			Container.Register<IRepositorioEscopo, DellaviaEscopo>(Lifestyle.Scoped);
			Container.Register<ILogIntegracaoServico, LogIntegracaoServico>(Lifestyle.Scoped);
			Container.Register<IOrcamentoServico, OrcamentoServico>(Lifestyle.Scoped);
			Container.Register<IVendedorServico, VendedorServico>(Lifestyle.Scoped);
			Container.Register<ILogCargaCatalogoServico, LogCargaCatalogoServico>(Lifestyle.Scoped);
			Container.Register<ICatalogoServico>(() => new CatalogoServico(escopo, ContainerGrafo.contextoGrafo, new Core.Servicos.GrupoProdutoServico(escopo)), Lifestyle.Scoped);
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
            Container.Register<ICargaProdutoServico>(() => new Core.Servicos.CargaProdutoServico(escopo, new CargaCadastrosSyncApi(), ContainerGrafo));
			Container.Register<ICargaCadastrosBasicosService>(() => new Core.Servicos.CargaCadastrosBasicosService(escopo, new CargaCadastrosSyncApi()));
			Container.Register<ICargaDescontosServico>(() => new Core.Servicos.CargaDescontosServico(escopo, new CargaCadastrosSyncApi()));
			Container.Register<ICargaClienteServico>(() => new Core.Servicos.CargaClienteServico(escopo, new CargaCadastrosSyncApi()));
            Container.Register<ICargaVeiculoServico>(() => new Core.Servicos.CargaVeiculoServico(escopo, dellaviaContexto));
            Container.Register<ICargaVeiculoProdutosServico>(() => new Core.Servicos.CargaVeiculosProdutoServico(escopo, dellaviaContexto));
            Container.Register<ICargaConvenioServico>(() => new Core.Servicos.CargaConvenioServico(escopo, new CargaCadastrosSyncApi()));
			Container.Register<ICargaPrecoServico>(() => new Core.Servicos.CargaPrecoServico(escopo, new CargaCadastrosSyncApi()));
			Container.Register<ICargaOrcamentoServico>(() => new Core.Servicos.CargaOrcamentoServico(escopo, new OrcamentoApi(escopo, dellaviaContexto, new CalculoImpostosApi(escopo)), new SolicitacaoAnaliseCreditoApi()));
			Container.Register<ICondicaoPagamentoParcelasApi, CondicaoPagamentoParcelasApi>(Lifestyle.Scoped);
			Container.Register<ICheckupCarServico<CheckupCarDto>, CheckupCarServico<CheckupCarDto>>(Lifestyle.Scoped);
			Container.Register<ICheckupTruckServico<CheckupTruckDto>, CheckupTruckServico<CheckupTruckDto>>(Lifestyle.Scoped);
			Container.Register<ICheckupServico<CheckupDto>, CheckupServico>(Lifestyle.Scoped);
			Container.Register<ICalculoImpostosApi, CalculoImpostosApi>(Lifestyle.Scoped);
			Container.Register<IHomeServico, HomeServico>(Lifestyle.Scoped);
            Container.Register<IImpostosServico, ImpostosServico>(Lifestyle.Scoped);
            Container.Register<IGrupoProdutoServico, GrupoProdutoServico>(Lifestyle.Scoped);
            //var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            //container.RegisterCollection(typeof(IValidator), assemblies);

            //adição de serviços
            //AutoMapperConfig.RegisterMappings();
            Container.Verify();
			DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(Container));
		}
	}
}