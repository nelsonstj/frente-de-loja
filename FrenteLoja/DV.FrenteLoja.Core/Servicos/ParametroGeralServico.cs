using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Exceptions;
using DV.FrenteLoja.Core.Extensions;
using DV.FrenteLoja.Core.Util;
using System;

namespace DV.FrenteLoja.Core.Servicos
{
    public class ParametroGeralServico : IParametroGeralServico
	{
		private readonly IRepositorio<ParametroGeral> _parametroGeralRepositorio;
		public ParametroGeralServico(IRepositorioEscopo escopo)
		{
			_parametroGeralRepositorio = escopo.GetRepositorio<ParametroGeral>();
		}

		public DateTime ObterDataVencimentoOrcamento()
		{
			var parametro = _parametroGeralRepositorio.GetSingle(x => x.CampoCodigo == Constants.DATA_LIMITE_ORC_PROTHEUS);
			if (parametro != null && !parametro.Descricao.IsNullOrEmpty())
			{
				try
				{
					var dias = Convert.ToInt32(parametro.Descricao);
					return DateTime.Now.AddDays(dias);

				}
				catch (Exception e)
				{
					throw new FormatException($@"O valor '{parametro.Descricao}' não pode ser convertido em um número.", e);
				}
			}
			throw new NegocioException("Impossível localizar valor no banco de dados.");
		}
	}
}