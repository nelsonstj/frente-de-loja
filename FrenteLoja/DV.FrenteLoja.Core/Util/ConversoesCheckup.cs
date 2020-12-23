using DV.FrenteLoja.Core.Contratos.Enums;
using System;

namespace DV.FrenteLoja.Core.Util
{
    public static class ConversoesCheckup
    {
        public static decimal? SubstituirSimbolosGraus(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return null;
            else
            {
                if (string.IsNullOrEmpty(texto.Replace("°", "").Replace("-", "").Replace("_", "")))
                    return null;
                else
                    return Convert.ToDecimal(texto.Replace('°', ',').Replace("_", ""));
            }
        }

        public static string ReverterSimbolosGraus(decimal? valor)
        {
            if (valor == null)
                return null;
            else
                return valor.ToString().Replace(',', '°');
        }

        public static TipoOleo SubstituirTipoOleo(string tipo)
        {
            switch (tipo)
            {
                case "Sintético":
                        return TipoOleo.Sintetico;
                case "Semi Sintético":
                    return TipoOleo.SemiSintetico;
                case "Mineral":
                    return TipoOleo.Mineral;
                default:
                    return TipoOleo.NaoSelecionado;
            }
        }
    }
}
