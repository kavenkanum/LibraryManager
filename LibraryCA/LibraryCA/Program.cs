using LibraryDomain;
using System;
using System.Linq;

namespace LibraryCA
{
	class Program
	{
		static void Main(string[] args)
		{


			using (var db = new BookRepository())
			{
				Console.WriteLine("Enter a name for your book");
				var name = Console.ReadLine();

				var book = new Book { Name = name };
				db.Books.Add(book);
				db.SaveChanges();

				var query = from b in db.Books
							orderby b.Name
							select b;

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
