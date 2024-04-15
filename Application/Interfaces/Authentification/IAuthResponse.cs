using Application.Dtos.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Authentification
{
    public interface IAuthResponse
    {
        Task<AuthResponse> SignUpAsync(SignUp model, string orgin);
        //for checking if the sent token is valid
        //Task<AuthResponse> RefreshTokenCheckAsync(string token);

        // for revoking refreshrokens
        //Task<bool> RevokeTokenAsync(string token);
        Task<AuthResponse> LoginAsync(Login model);
        Task<ClaimsPrincipal> DecodeJwtTokenAsync(string token);
    }
}
