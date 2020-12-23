using System.Web.Optimization;

namespace DV.FrenteLoja
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/libs/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/libs/jquery.validate*"));

            //// Use a versão em desenvolvimento do Modernizr para desenvolver e aprender. Em seguida, quando estiver
            //// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //"~/Scripts/libs/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                    "~/Scripts/libs/tether.min.js",
                    "~/Scripts/libs/popper.js",
                    "~/Scripts/libs/bootstrap.min.js",
                    "~/Scripts/libs/respond.js",
                    "~/Scripts/libs/select2.js",
                    "~/Scripts/libs/kendo.web.min.js",
                    "~/Scripts/libs/kendo.web.pt-BR.js",
                    "~/Scripts/libs/moment.js",
                    "~/Scripts/libs/jquery.mask.min.js",
                    "~/Scripts/libs/jquery.unobtrusive-ajax.js",
                    "~/Scripts/libs/jquery.maskMoney.js",
                    "~/Scripts/libs/polyfill.js",
                    "~/Scripts/libs/kendo-elasticsearch.js",
                    "~/Scripts/libs/html2canvas.js",
                    "~/Scripts/libs/jspdf.min.js",
                    "~/Scripts/libs/lodash.js",
                    "~/Scripts/libs/daterangepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                    "~/Scripts/app/componentes.js",
                    "~/Scripts/app/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/orcamento").Include(
                    "~/Scripts/app/orcamento/index.js",
                    "~/Scripts/app/orcamento/veiculo-cliente.js",
                    "~/Scripts/app/orcamento/equipe-vendas.js",
                    "~/Scripts/app/orcamento/busca-produto.js",
                    "~/Scripts/app/orcamento/busca-elasticsearch.js",
                    "~/Scripts/app/orcamento/orcamento.js",
                    "~/Scripts/app/orcamento/negociacao.js",
                    "~/Scripts/app/orcamento/finalizacao.js",
                    "~/Scripts/app/orcamento/relatorio.js"));

            bundles.Add(new ScriptBundle("~/bundles/checkup").Include(
                    "~/Scripts/app/checkup/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/home").Include(
                   "~/Content/app/home.css"));

            bundles.Add(new ScriptBundle("~/bundles/react").Include(
                "~/Scripts/libs/react.development.js",
                "~/Scripts/libs/react-dom.development.js",
                "~/Scripts/libs/browser.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/log-integracao").Include(
                    "~/Scripts/app/log-integracao/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/log-carga-catalogo").Include(
                "~/Scripts/app/log-carga-catalogo/index.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/libs/fontDelavia.css",
                    "~/Content/libs/bootstrap.css",
                    "~/Content/libs/select2.css",
                    "~/Content/libs/kendo.common.min.css",
                    "~/Content/libs/font-awesome.min.css",
                    "~/Content/libs/kendo.bootstrap.min.css",
                    "~/Content/libs/daterangepicker.css",
                    "~/Content/app/site.css",
					"~/Content/app/menu.css",
                    "~/Content/app/orcamento.css",
                    "~/Content/app/checkup.css"));

            bundles.Add(new ScriptBundle("~/bundles/inputmask").Include(
            "~/Scripts/Inputmask/inputmask.js",
            "~/Scripts/Inputmask/jquery.inputmask.js",
            "~/Scripts/Inputmask/inputmask.extensions.js",
            "~/Scripts/Inputmask/inputmask.date.extensions.js",
            "~/Scripts/Inputmask/inputmask.numeric.extensions.js"));
        }

    }
}
