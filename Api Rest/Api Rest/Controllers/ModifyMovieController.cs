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
	[RoutePrefix("api/ModifyMovie")]

	public class ModifyMovieController : ApiController
    {
		[Authorize(Users = "myuser")]
		[Route("ModifyMovies")]
		[HttpPost]
		public IHttpActionResult ModifyMovies(ModifyMovies modelo)
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
				"Name", modelo.name,
				"Id", modelo.id,
				"IdUser",modelo.id_user,
				"Accion",modelo.accion
			};



			try
			{
				conn.AbrirTransaccion();
				DataTable DtMov = (DataTable)conn.Ejecutar(parameters, "PutApiMovie");
				conn.CerrarTransaccion();

				oResult.Estatus = true;
				oResult.Message =  DtMov.Rows[0]["msg"].ToString();
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
