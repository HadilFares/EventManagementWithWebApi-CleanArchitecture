using Domain.Entities;
using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Context
{/*
    public class UserStore : IUserStore<User>, IUserPasswordStore<User>
    {
        private EventlyDbContext DataContext { get; init; }

        public UserStore(EventlyDbContext dataContext)
        {
            this.DataContext = dataContext;
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            // Ajout de l'utilisateur dans le contexte de données.
            this.DataContext.Add(user);

            // Persistance de l'ajout de l'utilisateur dans la base de données.
            await this.DataContext.SaveChangesAsync(cancellationToken);

            // Retour.
            return await Task.FromResult(IdentityResult.Success);
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            // Suppression de l'utilisateur dans le contexte de données.
            this.DataContext.Remove(user);

            // Persistance de la suppression de l'utilisateur dans la base de données.
            int result = await DataContext.SaveChangesAsync(cancellationToken);

            // Retour.
            return await Task.FromResult(result == 1 ? IdentityResult.Success : IdentityResult.Failed());
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            // Recherche d’un utilisateur à partir de son identifiant.
            return int.TryParse(userId, out int id) ?
                await DataContext.Users.FindAsync(id) :
                await Task.FromResult((User)null);
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            // Recherche d’un utilisateur à partir de son nom d’utilisateur.
            return await DataContext.Users
                .SingleOrDefaultAsync(u => u.UserName.Equals(normalizedUserName.ToLower()), cancellationToken);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                this.DataContext?.Dispose();
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.FromResult((object)null);
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;


            return Task.FromResult((object)null);
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        // Les autres méthodes à implémenter (provenant de l'interface IUserStore) lève une exception de type NotImplementedException car elles ne sont pas utilisées dans le cadre de cet article. Elles ne sont pas présentes ici pour simplifier le code.
    }
}
 
  */
}