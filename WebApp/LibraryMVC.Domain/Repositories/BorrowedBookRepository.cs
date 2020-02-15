using LibraryMVC.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryMVC.Domain.Repositories
{
    public interface IBorrowedBookRepository
    {
        bool Borrow(int bookId, int toUserId);
        bool Return(int borrowedBookId);
        int Find(int bookId);
        IEnumerable<Book> SelectBorrowedBooksByUser(int userId);
        IEnumerable<Book> SelectNotReturnedBorrowedBooksByUser(int userId);
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

        public bool Borrow(int bookId, int toUserId)
        {
            var book = _libraryDbContext.Books.Find(bookId);
            if (book != null && book.NumberAvailableBooks > 0)
            {
                DateTime timeOfBorrow = DateTime.UtcNow;
                var borrowedBook = new BorrowedBook()   //tworzę nowy obiekt
                {
                    UserId = toUserId,
                    BookId = bookId,
                    DateOfBorrow = timeOfBorrow
                };

                _libraryDbContext.Add(borrowedBook);
                book.NumberAvailableBooks--;
                _libraryDbContext.SaveChanges();
                return true;
            }
            return false;
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
                .Include(u => u.BorrowedBooks)
                .ThenInclude(bb => bb.Book)
                .Include($"{nameof(User.BorrowedBooks)}.{nameof(BorrowedBook.Book)}")
                .Select(u =>
                new UserWithBorrowedBooks
                {
                    User = u,
                    BorrowedBooks = u.BorrowedBooks.OrderBy(b=>b.Book.Name).ToList()
                })
                .ToList();
        }

        public bool Return(int borrowedBookId)
        {
            DateTime timeOfReturn = DateTime.UtcNow;
            var borrowedBook = _libraryDbContext.BorrowedBooks.SingleOrDefault(bb => bb.ID == borrowedBookId);

            borrowedBook.DateOfReturn = timeOfReturn; // dodaję do obiektu nowe rzeczy

            var returningBookId = from b in _libraryDbContext.BorrowedBooks
                                  where b.ID.Equals(borrowedBookId)
                                  select b.BookId;

            var returningBook = _libraryDbContext.Books.Find(returningBookId.FirstOrDefault());
            if (returningBook != null && returningBook.NumberAvailableBooks < returningBook.NumberAllBooks)
            {
                returningBook.NumberAvailableBooks++;
                _libraryDbContext.SaveChanges();
                return true;
            }
            return false; 
        }

        public int Find(int borrowedBookId)
        {
            return _libraryDbContext.BorrowedBooks.Where(b => b.ID == borrowedBookId).Select(b => b.BookId).FirstOrDefault();
        }

        //public IEnumerable<User> GetBorrowingUsers()
        //{
        //    var borrowingUsers = from u in _libraryDbContext.Users
        //                         where u.BorrowedBooks.Any()
        //                         select u;

        //    return borrowingUsers;
        //}

        public IEnumerable<Book> SelectNotReturnedBorrowedBooksByUser(int userId)
        {
            return _libraryDbContext.BorrowedBooks
                .Where(bb => bb.UserId == userId && bb.DateOfReturn == null)
                .Select(bb => bb.Book);
        }

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
