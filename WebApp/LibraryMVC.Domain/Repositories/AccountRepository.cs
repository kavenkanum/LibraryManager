using System;
using System.Collections.Generic;
using System.Text;
using LibraryMVC.Domain.Models;

namespace LibraryMVC.Domain.Repositories
{
    public interface IAccountRepository
    {
        void Add(Account newAccount);
        void Commit();
    }

    public class AccountRepository : IAccountRepository
    {
        private readonly LibraryDbContext _libraryDbContext;
        public AccountRepository(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
        }

        public void Add(Account newAccount)
        {
            _libraryDbContext.Accounts.Add(newAccount);
        }

        public void Commit()
        {
            _libraryDbContext.SaveChanges();
        }

    }
}
