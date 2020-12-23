using DV.FrenteLoja.Core.Servicos;
using DV.FrenteLoja.Core.Contratos.DataObjects;
using DV.FrenteLoja.Core.ProtheusAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DV.FrenteLoja.Core.Servicos.Tests
{
	[TestClass()]
	public class OrcamentoItemServicoTests
	{
		[TestMethod()]
		public void CalculaDescontoTest()
		{
			
			decimal resultado = OrcamentoItemServico.CalculaDesconto(150, 10)[0];
			Assert.AreEqual(135, resultado);
		}
		
	}
}

namespace UnitTest.Servicos
{
	[TestClass()]
	public class OrcamentoItemServicoTests
	{
		[TestMethod()]
		public void RemoverItensOrcamentoTest()
		{
			

		}
	}
}