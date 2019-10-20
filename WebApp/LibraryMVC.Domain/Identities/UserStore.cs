using LibraryMVC.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace LibraryMVC.Domain.Identities
{
    public class UserStore : IUserStore<Account>, IUserPasswordStore<Account>
    {
        private readonly IPasswordHasher<Account> _passwordHasher;

        private readonly LibraryDbContext _libraryDbContext;
        public UserStore(LibraryDbContext libraryDbContext, IPasswordHasher<Account> passwordHasher)
        {
            _libraryDbContext = libraryDbContext;
            _passwordHasher = passwordHasher;
        }
        public async Task<IdentityResult> CreateAsync(Account user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException();
            }
            if (user.Email != null && user.PasswordHash != null)
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, user.PasswordHash);
                await _libraryDbContext.AddAsync(user);
                await _libraryDbContext.SaveChangesAsync();
                return IdentityResult.Success;
            }
            else
            {
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> DeleteAsync(Account user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }
            _libraryDbContext.Accounts.Remove(user);
            await _libraryDbContext.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }

        public void Dispose()
        {
            _libraryDbContext.Dispose();
        }

        public Task<Account> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (int.TryParse(userId, out var parsedUserId))
            {
                return Task.FromResult(_libraryDbContext.Accounts.Where(a => a.ID == parsedUserId).SingleOrDefault());
            }

            return Task.FromResult(default(Account));
        }

        public Task<Account> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return Task.FromResult(_libraryDbContext.Accounts.Where(a => a.NormalizedUserName == normalizedUserName).SingleOrDefault());
            //return _libraryDbContext.Accounts.FindAsync((string)normalizedUserName);
        }

        public Task<string> GetNormalizedUserNameAsync(Account user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetPasswordHashAsync(Account user, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.PasswordHash);
        }

        public Task<string> GetUserIdAsync(Account user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.ID.ToString());
        }

        public Task<string> GetUserNameAsync(Account user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.UserName);
        }

        public Task<bool> HasPasswordAsync(Account user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task SetNormalizedUserNameAsync(Account user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.NormalizedUserName = normalizedName ?? throw new ArgumentNullException(nameof(user));
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(Account user, string passwordHash, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (passwordHash == null) throw new ArgumentNullException(nameof(passwordHash));

            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(Account user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.UserName = userName ?? throw new ArgumentNullException(nameof(user));
            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync(Account user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
