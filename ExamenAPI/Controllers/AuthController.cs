using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repository.DTO;
using ExamenAPI.Helpers;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;
using Repository.Interfaces;
using System.Diagnostics;
/*
Version: 1.0
Author: Pedro Pablo Soto
Description: Para seguridad, existen dos metodos que validan al usuario (por cuestion de pruebas, solo valida que UserName y Password no sean vacias), en un ambiente real, se debe actualizar
para que busque al usuario en la base de datos.

Segundo metodo, genera el token (JWT) con su respectiva clave secreta el cual tiene 30 minutos de validez
*/
namespace ExamenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogService logService;

        public AuthController(IConfiguration _configuration, ILogService _logService)
        {
            configuration = _configuration;
            logService = _logService;
        }

        [HttpPost]
        [Route("ValidateUser")]
        public ActionResult ValidateUser([FromBody] UserDTO model)
        {
            try
            {
                List<string> errors = new List<string>();

                if (model.UserName.Equals(""))
                {
                    errors.Add("User name is required.");
                }

                if (model.Password.Equals(""))
                {
                    errors.Add("Password is required.");
                }

                if (errors.Count > 0)
                {
                    return Ok(new ResponseDTO() { Status = ResponseStatus.Error, Message = "The User not exists.", Data = errors });
                }
                else
                {
                    return Ok(new ResponseDTO() { Status = ResponseStatus.Success, Message = "Success.", Data = model });
                }
            }
            catch(Exception error)
            {
                logService.Save(new LogDTO()
                {
                    Layer = "APIWEB",
                    FileName = "AuthController.cs",
                    MethodName = "ValidateUser",
                    Message = error.Message.ToString()
                });
                return Ok(new ResponseDTO() { Status = ResponseStatus.Error, Message = "Something goes wrong, please notify to the administrator." });
            }
        }

        [HttpPost]
        [Route("GenerateToken")]
        public ActionResult GenerateToken([FromBody] UserDTO model)
        {
            try
            {
                List<string> errors = new List<string>();

                if (model.UserName.Equals(""))
                {
                    errors.Add("User name is required.");
                }

                if (model.Password.Equals(""))
                {
                    errors.Add("Password is required.");
                }

                if (errors.Count > 0)
                {
                    return Ok(new ResponseDTO() { Status = ResponseStatus.Error, Message = "The User not exists.", Data = errors });
                }
                else
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity
                        (
                            new Claim[]
                            {
                                new Claim("Id", Guid.NewGuid().ToString()),
                                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()), //simulando el Id
                                new Claim(ClaimTypes.Name, model.UserName)
                            }
                        ),
                        Expires = DateTime.UtcNow.AddMinutes(30),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("SecretKey").Value)), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);

                    return Ok(new ResponseDTO() { Status = ResponseStatus.Success, Message = "Success.", Data = tokenHandler.WriteToken(token) });
                }
            }
            catch (Exception error)
            {
                logService.Save(new LogDTO()
                {
                    Layer = "APIWEB",
                    FileName = "AuthController.cs",
                    MethodName = "GenerateToken",
                    Message = error.Message.ToString()
                });
                return Ok(new ResponseDTO() { Status = ResponseStatus.Error, Message = "Something goes wrong, please notify to the administrator." });
            }
        }


    }
}
