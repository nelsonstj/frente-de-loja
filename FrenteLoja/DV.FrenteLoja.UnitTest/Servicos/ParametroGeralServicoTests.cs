using System;
using System.Diagnostics;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.Contratos.Enums;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;
using DV.FrenteLoja.Core.Servicos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace UnitTest.Servicos
{
	[TestClass()]
	public class ParametroGeralServicoTests
	{
		private IParametroGeralServico _parametroGeralRepositorio;
		//[TestInitialize]
		public void ParametroGeralServicoTestsInitialize()
		{
			BaseConfig.ConfigurarDependencias();
		}
		[TestMethod()]
		public void ObterDataVencimentoOrcamentoTest()
		{

			// arrange
			_parametroGeralRepositorio = new ParametroGeralServico(BaseConfig.Escopo);
			// act
			var dataVencimento = _parametroGeralRepositorio.ObterDataVencimentoOrcamento();

			// assert
			Assert.AreEqual(dataVencimento, DateTime.Now.AddDays(5));
		}
		[TestMethod()]
		public void DEVERA_OBTER_DESCRICAO_ENUM_NO_JSON()
		{
			
		}
	}
}