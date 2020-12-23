using System.Collections.Generic;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
    public interface ICatalogoProtheusApi
    {
        /// <summary>
        /// Método responsável por enviar para o Protheus as informações de catalogo.
        /// </summary>
        /// <param name="catalogoList">Lista de catalogos presentes na importação do *.csv.</param>
        /// <param name="erro">Erros do envio para o Protheus</param>
        /// <returns>Retorna a lista de objeto correspondente com o código Dellavia.</returns>
        List<Dominios.Entidades.Catalogo> PostCatalogo(List<Dominios.Entidades.Catalogo> catalogoList, out string erro);
    }
}