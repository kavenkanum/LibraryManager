using LibraryDomain;
using System;
using System.Linq;

namespace LibraryCA
{
	class Program
	{
		static void Main(string[] args)
		{


			using (var dbContext = new DbBook())
			{
				Console.WriteLine("Enter a name for your book");
				var name = Console.ReadLine();

				var book = new Book { Name = name };
				//db.Books.Add(book);
				//db.SaveChanges();

				var repository = new BookRepository(dbContext);
				repository.Add(book);
				repository.Commit();

				var query = from b in dbContext.Books
							orderby b.Name
							select b;

				Console.WriteLine("Number of books in repository");
				var numberOfBooks = repository.CountBooks();
				Console.WriteLine(numberOfBooks);

				Console.WriteLine("All books in the database");
				foreach (var item in query)
				{
					Console.WriteLine(item.Name);
				}

				Console.WriteLine("Press any key to exit...");
				Console.ReadKey();
			}

		}
	}
}
