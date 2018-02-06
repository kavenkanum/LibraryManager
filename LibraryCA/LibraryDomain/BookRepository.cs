using System.Data.Entity;
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

	public class BookRepository : DbBook
	{
		private readonly DbBook _dbBookContext;

		public BookRepository(DbBook dbBookContext)
		{
			_dbBookContext = dbBookContext;
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
	}
}
