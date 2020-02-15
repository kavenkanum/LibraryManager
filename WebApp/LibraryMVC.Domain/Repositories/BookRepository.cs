using System.Collections.Generic;
using System.Linq;
using LibraryMVC.Domain.Entities;

namespace LibraryMVC.Domain.Repositories
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetBooks();
        void Add(string name, string author, int numberAllBooks);
        Book SelectBook(Book selectedBook);
        bool Delete(int id);
        Book Find(int ID);
        void Edit(Book book);
        IEnumerable<Book> GetAvailableBooks();
        void Commit();

    }

    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _libraryDbContext;

        public BookRepository(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
        }

        public void AddOrUpdateNumberOfAvailableBooks(Book newBook)
        {
            var existingBook = _libraryDbContext.Books.SingleOrDefault(b => b.Name == newBook.Name && b.Author == newBook.Author);
            if (existingBook != null)
            {
                existingBook.NumberAvailableBooks += newBook.NumberAvailableBooks;
            }
            else
            {
                //Add(newBook);
            }
        }

        public void Add(string name, string author, int numberAllBooks)
        {
            var newBook = new Book();
            newBook.Name = name;
            newBook.Author = author;
            newBook.NumberAllBooks = numberAllBooks;
            newBook.NumberAvailableBooks = numberAllBooks;
            _libraryDbContext.Books.Add(newBook);
            _libraryDbContext.SaveChanges();
        }

        public bool Delete(int id)
        {
            var book = _libraryDbContext.Books.Find(id);
            if (book != null && book.NumberAllBooks == book.NumberAvailableBooks)
            {
                _libraryDbContext.Books.Remove(book);
                _libraryDbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public IEnumerable<Book> GetBooks()
        {
            return _libraryDbContext.Books.ToList();
        }

        public Book SelectBook(Book selectedBook)
        {
            return _libraryDbContext.Books.SingleOrDefault(p => p.Name == selectedBook.Name && p.Author == selectedBook.Author);
        }


        public Book Find(int id)
        {
            Book book = _libraryDbContext.Books.Find(id);
            return book;
        }

        public void Edit(Book book)
        {
            _libraryDbContext.Entry(book).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _libraryDbContext.SaveChanges();
        }

        public IEnumerable<Book> GetAvailableBooks()
        {
            return _libraryDbContext.Books.Where(b => b.NumberAvailableBooks>0).ToList();
        }

        public void Commit()
        {
            _libraryDbContext.SaveChanges();
        }
    }
}
