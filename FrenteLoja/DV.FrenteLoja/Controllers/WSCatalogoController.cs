using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using DV.FrenteLoja.Core.Contratos.Interfaces;
using DV.FrenteLoja.Core.Dominios.Entidades;

namespace DV.FrenteLoja.Controllers
{
	[Authorize]
    public class WSCatalogoController : ApiController
    {
        
        // POST: api/WSCatalogo
        public IHttpActionResult Post([FromBody]List<Catalogo> catalogos)
        {
	        if (catalogos == null)
		        return BadRequest("");
	        if (catalogos.Count == 0)
		        return StatusCode(HttpStatusCode.NoContent);

	        var catalogoProtheusApi = BaseConfig.Container.GetInstance<ICatalogoProtheusApi>();
	        var catalogoServico = BaseConfig.Container.GetInstance<ICatalogoServico>();
			var result = catalogoServico.ProcessaCatalogo(catalogos,catalogoProtheusApi);
	        if (string.IsNullOrEmpty(result))
	        {
		        return Ok();
			}
	        else
	        {
		        return InternalServerError(new Exception(result));
	        }
        }
		
    }
}
