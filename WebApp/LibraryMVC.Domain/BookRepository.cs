using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace LibraryDomain
{
	public class DbBook : DbContext
	{
		public DbBook()
		{
			Database.Connection.ConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Library;Integrated Security=True;";
			Database.Initialize(false);
		}

		public DbSet<Book> Books { get; set; }
	}

	public class BookRepository
	{
		private readonly DbBook _dbBookContext;

		public BookRepository(DbBook dbBookContext)
		{
			_dbBookContext = dbBookContext;
		}

		public void AddOrUpdateNumberOfAvailableBooks(Book newBook)
		{
			var existingBook = _dbBookContext.Books.SingleOrDefault(b => b.Name == newBook.Name && b.Author == newBook.Author);
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
			_dbBookContext.Books.Add(newBook);
		}

		public void Commit()
		{
			_dbBookContext.SaveChanges();
		}

		public void Delete(Book book)
		{
			_dbBookContext.Books.Remove(book);
		}

		public int CountBooks()
		{
			return _dbBookContext.Books.Count();
		}

		public IEnumerable<Book> GetBooks()
		{
			return _dbBookContext.Books.ToList();
		}

		public Book SelectBook(Book selectedBook)
		{
			return _dbBookContext.Books.SingleOrDefault(p => p.Name == selectedBook.Name && p.Author == selectedBook.Author);
		}
	
	}
}
