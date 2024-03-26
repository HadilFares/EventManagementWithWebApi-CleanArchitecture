using Application.Dtos.Account;
using Application.Interfaces.IBaseRepository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.AccountRepository
{
    public interface IAccountService:IBaseRepository<Account>
    {
        
        Task<List<Account>> GetPendingAccountsAsync();
        Task<bool> UpdateAccountStatusAsync(Guid accountId, AccountStatus status);
        Task  <bool>EditProfile(UserDTO userUpdateDTO);
       // Task<bool> IsUserOrganizerAsync(User user);

    }
}
