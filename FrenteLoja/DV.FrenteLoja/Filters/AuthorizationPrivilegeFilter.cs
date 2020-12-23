using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Infra.Security;

namespace DV.FrenteLoja.Filters
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class AuthorizationFilter : AuthorizeAttribute, IAuthorizationFilter
	{
		public PerfilAcessoUsuario[] FlagPerfilAcessoUsuario { get; set; }

		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			return Process();
		}

		private bool Process()
		{
			try
			{
				return FlagPerfilAcessoUsuario.Any(p => p.Equals(HttpContext.Current.User.Identity.GetPerfilAcessoUsuario()));
            }
			catch (Exception)
			{
				return false;
			}
		}

	}
}