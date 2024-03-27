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
