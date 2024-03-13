using Application.Dtos.Account;
using Application.Interfaces.Authentification;
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
       

        [HttpPost("signUp")]
        public async Task<IActionResult> SignUpAsync([FromForm] SignUp model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var orgin = Request.Headers["origin"];
            var result = await _authService.SignUpAsync(model, orgin);

            if (!result.ISAuthenticated)
                return BadRequest(result.Message);

            //store the refresh token in a cookie
           // SetRefreshTokenInCookies(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }
    }
}
