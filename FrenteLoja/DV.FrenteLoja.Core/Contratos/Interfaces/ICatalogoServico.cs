using System.Collections.Generic;
using System.Linq;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Core.Contratos.Interfaces
{
	public interface ICatalogoServico 
	{
		void UploadArquivoCatalogo(CargaCatalogoDto catalogoDto, ICatalogoProtheusApi catalogoProtheusApi);

		string ProcessaCatalogo(List<Catalogo> catalogoList, ICatalogoProtheusApi catalogoProtheusApi);

		IQueryable<CatalogoDto> Obter();
		
		IQueryable<string> ObterFabricantePecaPorTermo(string termoBusca, int tamanhoPagina, int numeroPagina);
		int TamanhoTermoFabricantePeca(string termoBusca);
        void ProcessaCatalogoHistoricoProtheus();

    }
}
