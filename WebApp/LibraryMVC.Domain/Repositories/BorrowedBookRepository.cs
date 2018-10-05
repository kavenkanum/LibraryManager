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
        void Return(int borrowedBookId);
        void DecreaseNumberOfAvailableBooks(int bookId);
        IEnumerable<Book> SelectBorrowedBooksByUser(int userId);
        //IEnumerable<User> GetBorrowingUsers();
        IEnumerable<UserWithBorrowedBooks> GetUsersWithBorrowedBooks();
        IEnumerable<UserWithBorrowedBooks> ListOfAllBorrowedBooks();

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
            DateTime timeOfBorrow = DateTime.UtcNow;
            var borrowedBook = new BorrowedBook()   //tworzę nowy obiekt
            {
                UserId = toUserId,
                BookId = bookId,
                DateOfBorrow = timeOfBorrow
            };

            _libraryDbContext.Add(borrowedBook);
            DecreaseNumberOfAvailableBooks(bookId);
            _libraryDbContext.SaveChanges();
        }

        public IEnumerable<UserWithBorrowedBooks> GetUsersWithBorrowedBooks()
        {
            return _libraryDbContext.Users
                .Include($"{nameof(User.BorrowedBooks)}.{nameof(BorrowedBook.Book)}")
                //.Include("BorrowedBooks.Book")
                .Where(b => b.BorrowedBooks.Any(bb => bb.DateOfReturn == null))
                .Select(u =>
                new UserWithBorrowedBooks
                {
                    User = u,
                    BorrowedBooks = u.BorrowedBooks.Where(bb => bb.DateOfReturn == null).ToList()
                })
                .ToList();
        }

        public IEnumerable<UserWithBorrowedBooks> ListOfAllBorrowedBooks()
        {
            return _libraryDbContext.Users
                .Include($"{nameof(User.BorrowedBooks)}.{nameof(BorrowedBook.Book)}")
                .Select(u =>
                new UserWithBorrowedBooks
                {
                    User = u,
                    BorrowedBooks = u.BorrowedBooks.OrderBy(b=>b.Book.Name).ToList()
                })
                .ToList();
        }

        public void Return(int borrowedBookId)
        {
            DateTime timeOfReturn = DateTime.UtcNow;
            var borrowedBook = _libraryDbContext.BorrowedBooks.SingleOrDefault(bb => bb.ID == borrowedBookId);

            borrowedBook.DateOfReturn = timeOfReturn; // dodaję do obiektu nowe rzeczy

            var returningBookId = from b in _libraryDbContext.BorrowedBooks
                                  where b.ID.Equals(borrowedBookId)
                                  select b.BookId;

            var returningBook = _libraryDbContext.Books.Find(returningBookId.FirstOrDefault());
            returningBook.NumberAvailableBooks++;
            
            _libraryDbContext.SaveChanges();
        }


        public void DecreaseNumberOfAvailableBooks(int bookId)
        {
            var borrowedBook = _libraryDbContext.Books.Find(bookId);

            if (borrowedBook != null)
            {
                borrowedBook.NumberAvailableBooks--;
            }
        }

        //public IEnumerable<User> GetBorrowingUsers()
        //{
        //    var borrowingUsers = from u in _libraryDbContext.Users
        //                         where u.BorrowedBooks.Any()
        //                         select u;

        //    return borrowingUsers;
        //}

        public IEnumerable<Book> SelectBorrowedBooksByUser(int userId)
        {
            var booksBorrowedByUser = _libraryDbContext.BorrowedBooks
                .Where(bb => bb.UserId == userId)
                .Select(bb => bb.Book);

            //var books = from b in _libraryDbContext.Books
            //            where b.BorrowingUsers.Any(bb => bb.UserId == userId)
            //            select b;

            return booksBorrowedByUser;
        }

    }

    public class UserWithBorrowedBooks
    {
        public User User { get; set; }
        public List<BorrowedBook> BorrowedBooks { get; set; }
    }
}
