using DV.FrenteLoja.Core.Servicos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.ProtheusAPI
{
	[TestClass()]
	public class CargaCadastrosProtheusSyncApiTests
	{
		[TestMethod()]
		public async void DEVERA_RETORNAR_LISTA_CONVENIOS()
		{
			var service = new CargaCadastrosBasicosService();
			await service.SyncConvenios();
			Assert.Fail();
		}
	}
}