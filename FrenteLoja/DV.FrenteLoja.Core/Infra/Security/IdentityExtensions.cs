using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Infra.Security
{
	public static class IdentityExtensions
	{
		public static string GetName(this IIdentity identity)
		{
			var claim = ((ClaimsIdentity)identity).FindFirst("Name");
			// Test for null to avoid issues during local testing
			return (claim != null) ? claim.Value : string.Empty;
		}

		public static PerfilAcessoUsuario GetPerfilAcessoUsuario(this IIdentity identity)
		{
			var claim = ((ClaimsIdentity)identity).FindFirst("PerfilAcesso");
			return claim != null ? (PerfilAcessoUsuario)Convert.ToInt32(claim.Value): PerfilAcessoUsuario.TMK;
		}

        public static string GetAreaNegocio(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("TipoVenda");
            return claim != null ? claim.Value : string.Empty;
        }

        public static string GetTipoVenda(this IIdentity identity)
		{
			var claim = ((ClaimsIdentity)identity).FindFirst("TipoVenda");
			return claim != null ? claim.Value : string.Empty;
		}

		public static string GetConvenioPadrao(this IIdentity identity)
		{
			var claim = ((ClaimsIdentity)identity).FindFirst("ConvenioPadrao");
			return (claim != null) ? claim.Value : string.Empty;
		}
		public static string GetLojaPadraoCampoCodigo(this IIdentity identity)
		{
			var claim = ((ClaimsIdentity)identity).FindFirst("LojaPadraoCampoCodigo");
			return (claim != null) ? claim.Value : string.Empty;
		}
		public static List<string> GetLojaPadrao(this IIdentity identity)
		{
			var claim = ((ClaimsIdentity)identity).FindFirst("LojaPadraoCampoCodigo");
            var ids = new List<string>();
            if (claim != null)
                foreach (var item in claim.Value.Split(','))
                    ids.Add(item);
            return ids;
		}
        public static string GetIdOperador(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("IdOperador");
			return (claim != null) ? claim.Value : string.Empty;
        }
		public static string GetEmailOperador(this IIdentity identity)
		{
			var claim = ((ClaimsIdentity)identity).FindFirst("Email");
			return (claim != null) ? claim.Value : string.Empty;
		}
	}
}