//using DB.SQLServer.Config;
//using DB.SQLServer.Infraestrutura;

using System;
using System.Web.Mvc;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Grafo;
using DV.FrenteLoja.Core.Infra.EntityFramework;
using DV.FrenteLoja.Core.ProtheusAPI;
using DV.FrenteLoja.Core.Servicos;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Lifestyles;

namespace UnitTest
{
	public class BaseConfig
	{
		public static readonly Container Container = new Container();
		public static ConfigGrafo ContainerGrafo;
		public static IRepositorioEscopo Escopo;
		public static void ConfigurarDependencias()
		{

			//grafo
			//ContainerGrafo = new ConfigGrafo(ConfigurationManager.ConnectionStrings["DellaviaGrafoContexto"].ConnectionString);
			//ContainerGrafo.Conectar(Core.Grafo.Enumerator.TipoConexao.Sincrona);

			
			Escopo = new DellaviaEscopo(new DellaviaContexto());

			//contexto
			Container.Options.DefaultScopedLifestyle = new ThreadScopedLifestyle();


			Container.Register<DellaviaContexto>(Lifestyle.Scoped);
			Container.Register<IRepositorioEscopo, DellaviaEscopo>(Lifestyle.Scoped);
			Container.Register<ILogIntegracaoServico, LogIntegracaoServico>(Lifestyle.Scoped);
			Container.Register<IOrcamentoServico, OrcamentoServico>(Lifestyle.Scoped);
			Container.Register<IVendedorServico, VendedorServico>(Lifestyle.Scoped);
			Container.Register<ILogCargaCatalogoServico, LogCargaCatalogoServico>(Lifestyle.Scoped);
			//Container.Register<ICatalogoServico>(() => new CatalogoServico(escopo, ContainerGrafo.contextoGrafo), Lifestyle.Scoped);
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
			Container.Register<ICargaProdutoServico>(() => new DV.FrenteLoja.Core.Servicos.CargaProdutoServico(Escopo, new CargaCadastrosSyncApi(), ContainerGrafo));
			Container.Register<ICargaCadastrosBasicosService>(() => new DV.FrenteLoja.Core.Servicos.CargaCadastrosBasicosService(Escopo, new CargaCadastrosSyncApi()));
            Container.Register<IVeiculoServico, VeiculoServico>(Lifestyle.Scoped);

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			//container.RegisterCollection(typeof(IValidator), assemblies);

			//adição de serviços
			//AutoMapperConfig.RegisterMappings(container);
			Container.Verify();
			DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(Container));
		}
	}
}