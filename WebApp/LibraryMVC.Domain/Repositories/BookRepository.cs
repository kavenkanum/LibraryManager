using System.Collections.Generic;
using System.Linq;
using LibraryMVC.Domain.Models;

namespace LibraryMVC.Domain.Repositories
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetBooks();
        void Add(Book newBook);
        void Commit();
        Book SelectBook(Book selectedBook);
        void Delete(Book book);
        Book Find(int ID);
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
                Add(newBook);
            }
        }

        public void Add(Book newBook)
        {
            _libraryDbContext.Books.Add(newBook);
        }

        public void Commit()
        {
            _libraryDbContext.SaveChanges();
        }

        public void Delete(Book book)
        {
            _libraryDbContext.Books.Remove(book);
        }

        public int CountBooks()
        {
            return _libraryDbContext.Books.Count();
        }

        public IEnumerable<Book> GetBooks()
        {
            return _libraryDbContext.Books.ToList();
        }

        public Book SelectBook(Book selectedBook)
        {
            return _libraryDbContext.Books.SingleOrDefault(p => p.Name == selectedBook.Name && p.Author == selectedBook.Author);
        }

        public Book Find(int ID)
        {
            Book book = _libraryDbContext.Books.Find(ID);
            return book;
        }
    }
}
