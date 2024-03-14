using Application.Dtos.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Authentification
{
    public interface IAuthResponse
    {
        Task<AuthResponse> SignUpAsync(SignUp model, string orgin);
       
    }
}
