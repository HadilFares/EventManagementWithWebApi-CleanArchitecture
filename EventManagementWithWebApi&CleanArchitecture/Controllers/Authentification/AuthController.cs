using Application.Dtos.Account;
using Application.Interfaces.Authentification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementWithWebApi_CleanArchitecture.Controllers.Authentification
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthResponse _authService;

        public AuthController(IAuthResponse authService)
        {
            _authService = authService;
        }

        [Authorize]
        [HttpGet("decode")]
        public async Task<IActionResult> DecodeToken(string token)
        {
            var principal = await _authService.DecodeJwtTokenAsync(token);

            if (principal != null)
            {
                // Initialize variables to store the extracted values
                string email = string.Empty;
                string username = string.Empty;
                string id = string.Empty;
                List<string> roles = new List<string>();


                // Extract the relevant claims from the principal
                foreach (var claim in principal.Claims)
                {
                    switch (claim.Type)
                    {
                        case "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress":
                            email = claim.Value;
                            break;
                        case "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name":
                            username = claim.Value;
                            break;
                        case "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier":
                            id = claim.Value;
                            break;
                        case "http://schemas.microsoft.com/ws/2008/06/identity/claims/role":
                            roles.Add(claim.Value);
                            break;
                        default:
                            break;
                    }
                }

                // Create a custom object to hold the extracted values
                var userDetails = new
                {
                    Email = email,
                    Username = username,
                    ID = id,
                    Roles = roles.ToList(),
                    ISAuthenticated=true
                };

                return Ok(userDetails);
            }
            else
            {
                return BadRequest("Token validation failed.");
            }
        }



        [HttpPost("signUp")]
       
        public async Task<IActionResult> SignUpAsync([FromBody] SignUp model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var orgin = Request.Headers["origin"];
            var result = await _authService.SignUpAsync(model, orgin);
            return Ok(result);
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] Login model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(model);

            if (!result.ISAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

    }
}
