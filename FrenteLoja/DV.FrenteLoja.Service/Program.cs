using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;

namespace DV.FrenteLoja.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] services = null;
            BaseConfig.ConfigurarDependencias();

            /* Não remover */

            //if (System.Environment.MachineName.Equals("BNU-0463") && !Debugger.IsAttached)
            //{
            //	Thread.Sleep(TimeSpan.FromSeconds(10)); //for debug.
            //}

            services = new ServiceBase[]
                {

                     
                     new CargaCadastrosBasicosServico(),
                    // new CargaConvenioServico(),
                    // new CargaPrecoServico(),
                    // new CargaProdutoServico(),
                    //new CargaClienteServico(),
                    // new CargaDescontosServico(),
                    // new CargaOrcamentoService(),
                };

            //ServiceBase myService = new CargaCadastrosBasicosServico();
            //myService.ServiceName = "CargaCadastrosBasicosServico";
            //ServiceBase.Run(myService);

            //ServiceBase.Run(new CargaCadastrosBasicosServico());
            ServiceBase.Run(services);
        }
    }
}
