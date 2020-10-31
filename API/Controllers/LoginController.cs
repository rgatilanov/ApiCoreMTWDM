using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        readonly IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usMin">{"Nick": "rgatilanov","Password": "96CAE35CE8A9B0244178BF28E4966C2CE1B8385723A96A6B838858CDD6CA0A1E"}</param>
        /// <returns></returns>
        [HttpPost]
        #region Método de simulación (sin conexión a BD ) de login

        public Login Authenticate(UserMin usMin)
        {
            // Integración a base de datos
            if (usMin.Nick == "rgatilanov" && usMin.Password == "96CAE35CE8A9B0244178BF28E4966C2CE1B8385723A96A6B838858CDD6CA0A1E") //SHA2
            {
                // Leemos el secret_key desde nuestro appseting
                var secretKey = _configuration.GetValue<string>("SecretKey");
                var key = Encoding.ASCII.GetBytes(secretKey);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    // Nuestro token va a durar un día
                    Expires = DateTime.UtcNow.AddDays(1),
                    // Credenciales para generar el token usando nuestro secretykey y el algoritmo hash 256
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var createdToken = tokenHandler.CreateToken(tokenDescriptor);

                return new Login()
                {
                    ID = usMin.ID,
                    Nick = usMin.Nick,
                    Token = tokenHandler.WriteToken(createdToken),
                };
            }
            else
                return null;

        }
        #endregion
        #region Método para integración con Angular
        /*public IActionResult Login([FromBody] UserMin user)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }

            if (user.Nick == "rgatilanov" && user.Password == "4297f44b13955235245b2497399d7a93") //MD5 (123123)
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("maestria-mtwdm-2019"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokeOptions = new JwtSecurityToken(
                    issuer: "http://localhost:44369",
                    audience: "http://localhost:44369",
                    claims: new List<System.Security.Claims.Claim>(),
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new { Token = tokenString });
            }
            else
            {
                return Unauthorized();
            }
        }*/
        #endregion
    }
}
