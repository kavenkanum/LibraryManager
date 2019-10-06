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
        string EncryptPassword(string password);
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

        public string EncryptPassword(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            UTF32Encoding encoder = new UTF32Encoding();
            var originalBytes = encoder.GetBytes(password);
            var encodedBytes = md5.ComputeHash(originalBytes);
            var result = BitConverter.ToString(encodedBytes);
            return result;
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
