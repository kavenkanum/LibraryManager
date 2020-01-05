using System;
using System.Security.Cryptography;
using System.Text;
using LibraryMVC.Domain.Entities;

namespace LibraryMVC.Domain.Repositories
{
    public interface IAccountRepository
    {
        void Add(Account newAccount);
        void Commit();
        
        //bool LoginSuccess(string email, string password);
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


        //public bool LoginSuccess(string email, string password)
        //{
        //    //var success = _libraryDbContext.Accounts.Where(a => a.Email.Equals(email) && a.Password.Equals(EncryptPassword(password))).FirstOrDefault();
        //    //if (success != null)
        //    //{
        //    //    return true;
        //    //}
        //    //return false;
        //}
    }
}
