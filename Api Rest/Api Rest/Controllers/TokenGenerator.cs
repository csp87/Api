using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.Security.Claims;

namespace Api_Rest.Controllers
{
    /// <summary>
    /// JWT Token generator class using "secret-key"
    /// more info: https://self-issued.info/docs/draft-ietf-oauth-json-web-token.html
    /// </summary>
    internal static class TokenGenerator
    {
        public static string GenerateTokenJwt(string userName,string userIdentifier)
        {
            // appsetting for Token JWT
            var secretKey = ConfigurationManager.AppSettings["KEY_TOKEN"];
            var audienceToken = ConfigurationManager.AppSettings["AUDIENCE_TOKEN"];
            var issuerToken = ConfigurationManager.AppSettings["ISSUER_TOKEN"];
            var expireTime = ConfigurationManager.AppSettings["EXPIRE_MINUTES_TOKEN"];

            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // create a claimsIdentity
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.NameIdentifier, userIdentifier)
                }) ;

            // create token to the user
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                audience: audienceToken,
                issuer: issuerToken,
                subject: claimsIdentity,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(expireTime)),
                signingCredentials: signingCredentials);

            var jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);
            return jwtTokenString;
        }
    }
}