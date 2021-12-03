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
    [RoutePrefix("api/loginserver")]
    public class LoginServidorController : ApiController
    {
        [Authorize(Users = "myuser")]
        [Route("Login")]
        [HttpPost]
        public IHttpActionResult Login(CreateUser modelo)
        {
            Result oResult = new Result();

            if (!ModelState.IsValid)
            {
                oResult.Estatus = false;
                oResult.Message = "Error modelo de datos. " + BadRequest(ModelState).ToString();
                return Ok(oResult);

            }
                ConexionBD conn = new ConexionBD();
            try
            {
                object[] parameters = {
                "email", modelo.email,
                "password", modelo.password,
                };

                
                DataTable dtusu = (DataTable)conn.Ejecutar(parameters, "loginusuario");

                if (dtusu.Rows.Count > 0)
                {
                    oResult.Estatus = true;
                    oResult.Message = "User logged in correctly";
                    return Ok(oResult);
                }
                else 
                {
                    oResult.Estatus = false;
                    oResult.Message = "Please verify username or password";
                    return Ok(oResult);
                }

                
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
