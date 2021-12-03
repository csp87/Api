namespace Api_Rest.Controllers
{
    using Api_Rest.Models;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Configuration;
    using System.Web.Http;
    

    /// <summary>
    /// login controller class for authenticate users
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/Login")]
    public class LoginController : ApiController
    {
        [HttpPost]
        [Route("Autenticar")]
        public IHttpActionResult Autenticar(Login model)
        {
            Result oResult = new Result();
            if (!ModelState.IsValid)
            {
                oResult.Estatus = false;
                oResult.Message = "Data model error. " + BadRequest(ModelState).ToString();
                return Ok(oResult);
            }
            try
            {
                ////string usr = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Name).Value;
                string usr = ConfigurationManager.AppSettings["usr"];
                string pass = ConfigurationManager.AppSettings["pass"];   

                if (model.usr == usr && model.pass  == pass)
                {
                    oResult.Estatus = true;
                    oResult.Message = "Generated Token";
                    oResult.Data = TokenGenerator.GenerateTokenJwt(model.usr, "1");
                    return Ok(oResult);
                }
                else
                {
                    oResult.Estatus = false ;
                    oResult.Message = "Invalid User or password";
                    return Ok(oResult);
                }
            }
            catch (Exception ex)
            {
                oResult.Estatus = false;
                oResult.Message = "Login error.";
                return Ok(oResult);
            }
        }

    }
}
