using Api_Rest.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api_Rest.Controllers
{
	[RoutePrefix("api/GetMovie")]
	public class GetMoviesController : ApiController
    {
		[Authorize(Users = "myuser")]
		[Route("GetMovies")]
		[HttpPost]

		public IHttpActionResult GetMovies(GetMovies modelo)
		{
			Result oResult = new Result();

			if (!ModelState.IsValid)
			{
				oResult.Estatus = false;
				oResult.Message = "Data model error. " + BadRequest(ModelState).ToString();
				return Ok(oResult);

			}
			ConexionBD conn = new ConexionBD();
			object[] parameters = {
				"Id", modelo.id,
				"IdUser", modelo.idUser
			};



			try
			{
				conn.AbrirTransaccion();
				DataTable DtMov = (DataTable)conn.Ejecutar(parameters, "GetApiMovie");
				conn.CerrarTransaccion();
				oResult.Data = DtMov;
				oResult.Estatus = true;
				/*oResult.Message = "user created successfully" + DtMov.Rows[0]["mensaje"].ToString();*/
				return Ok(oResult);
			}
			catch (Exception ex)
			{
				conn.DevolerTransaccion();
				oResult.Estatus = false;
				oResult.Message = "Startup error " + ex.Message;
				return Ok(oResult);
			}
			finally
			{
				conn.Close();
			}
		}
	}
}
