using Api_Rest.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace Api_Rest.Controllers
{
    [RoutePrefix("api/SaveUsr")]
    public class ModifyUserController : ApiController
    {
        
        [Authorize(Users = "myuser")]
        [Route("CreateUser")]
        [HttpPost]
        public IHttpActionResult CreateUser(CreateUser modelo)
        {
            Result oResult = new Result();

            if (!ModelState.IsValid)
            {
                oResult.Estatus = false;
                oResult.Message = "Data model error. " + BadRequest(ModelState).ToString();
                return Ok(oResult);

            }

            if (!MailValidate(modelo.email)) {
                oResult.Estatus = false;
                oResult.Message = "The email entered is not valid";
                return Ok(oResult);
            }

            if (modelo.password.ToString().Length < 10) {
                oResult.Estatus = false;
                oResult.Message = "The password must contain a minimum of 10 characters";
                return Ok(oResult);
            }

            if (!ValidateMin(modelo.password) || !ValidateMayus(modelo.password))
            {
                oResult.Estatus = false;
                oResult.Message = "The password must contain at least one lower case and one upper case";
                return Ok(oResult);
            }

            if (ValidateMail(modelo.email))
            {
                oResult.Estatus = false;
                oResult.Message = "The email entered is already registered";
                return Ok(oResult);
            }
            ConexionBD conn = new ConexionBD();
            object[] parameters = {
                "Email", modelo.email,
                "Password", modelo.password,
				"Accion", modelo.accion

			};
       
            try
            {
                conn.AbrirTransaccion();
                DataTable DtUser = (DataTable)conn.Ejecutar(parameters, "PutUser");
                conn.CerrarTransaccion();

                oResult.Estatus = true;
                oResult.Message = "User created successfully" + DtUser.Rows[0]["msg"].ToString();
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

        private bool ValidateMail(string email)
        {
            ConexionBD conn = new ConexionBD();
            object[] parameters = {
                "email",email,
            };


            
            DataTable dtusu = (DataTable)conn.Ejecutar(parameters, "GetCheckMail");
            if (dtusu.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

            
        }

        private bool ValidateMin(string texto)
        {
            
            string letras = "abcdefghyjklmnñopqrstuvwxyz";
            
            for (var i = 0; i < texto.Length; i++)
            {
                if (letras.IndexOf(texto.Substring(i, 1), 0) != -1)
                {
                    return true;
                }
            }
            return false;
        }

        private bool ValidateMayus(string texto)
        {
            string letras_mayusculas = "ABCDEFGHYJKLMNÑOPQRSTUVWXYZ";
            for (var i = 0; i < texto.Length; i++)
            {
                if (letras_mayusculas.IndexOf(texto.Substring(i, 1), 0) != -1)
                {
                    return true;
                }
            }
            return false;
        }

        private bool MailValidate(string email) 
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

       
    }
}
