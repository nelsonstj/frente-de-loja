using System.Web;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Infra.Security;

namespace DV.FrenteLoja.Core.Util
{
    public static class FiltroHelper
	{
		public static bool IsTMK()
		{
			return HttpContext.Current.User.Identity.GetPerfilAcessoUsuario() == PerfilAcessoUsuario.TMK;
		}

		public static bool IsLoja()
		{
			return HttpContext.Current.User.Identity.GetPerfilAcessoUsuario() == PerfilAcessoUsuario.FrenteLoja;
		}

        public static string UrlElasticSearch()
        {
            return System.Configuration.ConfigurationManager.AppSettings["ElasticSearchAddress"];
        }

        public static string UrlImagemDellaVia()
        {
            return System.Configuration.ConfigurationManager.AppSettings["DellaViaAddress"];
        }

        public static string UrlLogoDellaVia()
        {
            return System.Configuration.ConfigurationManager.AppSettings["LogoDellaViaAddress"];
        }
    }
}