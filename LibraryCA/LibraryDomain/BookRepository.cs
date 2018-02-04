using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDomain
{
	public class DbBook : DbContext
	{
		public DbSet<Book> Books { get; set; }
	}

	public class BookRepository : DbBook
	{

		DbContext _ctx;
		DbSet<Book> _set;

		public void Add(Book newBook)
		{
			_set.Add(newBook);
		}

		public void Commit()
		{
			_ctx.SaveChanges();
		}

		public void Delete()
		{

		}
	}
}
