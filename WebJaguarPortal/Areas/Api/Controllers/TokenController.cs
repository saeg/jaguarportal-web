using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebJaguarPortal.Areas.Api.Models;
using WebJaguarPortal.Services;

namespace WebJaguarPortal.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly TokenService tokenService;
        private readonly UserService userService;

        public TokenController(TokenService tokenService, UserService userService)
        {
            this.tokenService = tokenService;
            this.userService = userService;
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Models.TokenModelResponse))]
        [ProducesResponseType(400, Type = typeof(Models.TokenErrorModelResponse))]
        [ProducesResponseType(401, Type = typeof(Models.TokenErrorModelResponse))]

        public IActionResult Post([FromForm] TokenModelRequest request)
        {
            if (request.grant_type != "client_credentials")
            {
                return BadRequest(new TokenErrorModelResponse() { error = "unsupported_grant_type", error_description = "Unsupported grant_type, use client_credentials." });
            }

            var user = userService.ValidateClientCredentials(request.client_id, request.client_secret);

            if (user == null)
            {
                return Unauthorized(new TokenErrorModelResponse() { error = "invalid_client", error_description = "Client authentication failed, such as if the request contains an invalid client ID or secret." });
            }
            else
            {
                IEnumerable<System.Security.Claims.Claim> claim = userService.GenerateClaims(user);
                string token = tokenService.GenerateToken(user, claim);

                return Ok(new TokenModelResponse()
                {
                    access_token = token,
                    expires_in = 1200,
                    token_type = JwtBearerDefaults.AuthenticationScheme,
                    username = user.Username
                });
            }
        }
    }
}
