using Application.Dtos.Accounts;
using Application.Interfaces.IBaseRepository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.AccountRepository
{
    public interface IAccountService
    {
        Task<Account> GetAccount(Guid id);
        Task<List<Account>> GetAllAccounts();
        Task<List<(Account account, string role)>> GetPendingOrganizerAndParticipantAccountsAsync();
        Task<List<Account>> GetPendingExhibitorAccountsAsync();
        Task<bool> UpdateAccountStatusAsync(Guid accountId, AccountStatus status);
        Task  <bool>EditProfile(UserDTO userUpdateDTO);
     

    }
}
