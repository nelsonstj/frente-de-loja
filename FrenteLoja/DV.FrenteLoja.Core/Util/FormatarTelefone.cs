using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DV.FrenteLoja.Core.Util
{
    public static class FormatarTelefone
    {
        public static string Formatar(string telefone)
        {
            if (string.IsNullOrEmpty(telefone))
                return string.Empty;

            telefone = telefone.Replace("-", "");
            switch (telefone.Length)
            {
                case 7:
                    telefone = "000"+telefone;break;
                case 8:
                    telefone = "000" + telefone; break;
                case 10:
                    telefone = telefone.First().Equals('0') ? telefone : "0" + telefone; break;
                case 11:
                    telefone = telefone.First().Equals('0') ? telefone : "0" + telefone; break;              
                default:
                    break;
            }
            return telefone;
        }
    }
}
