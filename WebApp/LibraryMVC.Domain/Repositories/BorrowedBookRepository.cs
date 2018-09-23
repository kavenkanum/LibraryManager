using LibraryMVC.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryMVC.Domain.Repositories
{
    public interface IBorrowedBookRepository
    {
        void Borrow(int bookId, int toUserId);
        void Return(int bookId, int fromUserId);
        void DecreaseNumberOfAvailableBooks(int bookId);
        IEnumerable<User> GetBorrowingUsers();
    }

    public class BorrowedBookRepository : IBorrowedBookRepository
    {
        private readonly LibraryDbContext _libraryDbContext;

        public BorrowedBookRepository(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
        }
        
        public void Borrow(int bookId, int toUserId)
        {
            var borrowedBook = new BorrowedBook()
            {
                UserId = toUserId,
                BookId = bookId
            };

            _libraryDbContext.Add(borrowedBook);
            DecreaseNumberOfAvailableBooks(bookId);
            _libraryDbContext.SaveChanges();
        }

        public IEnumerable<User> GetBorrowingUsers()
        {
            var borrowingUsers = from u in _libraryDbContext.Users
                                 where u.BorrowedBooks.Any()
                                 select u;

            return borrowingUsers;
        }

        public void Return(int bookId, int fromUserId)
        {
            var borrowedBook = _libraryDbContext.BorrowedBooks.SingleOrDefault(bb => bb.BookId == bookId && bb.UserId == fromUserId);
            _libraryDbContext.Remove(borrowedBook);
            _libraryDbContext.SaveChanges();
        }


        public void DecreaseNumberOfAvailableBooks(int bookId)
        {
            var borrowedBook = _libraryDbContext.Books.Find(bookId);
           
            if (borrowedBook != null)
            {
                borrowedBook.NumberAvailableBooks --;
            }
        }
    }
}
