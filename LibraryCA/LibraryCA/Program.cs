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
				var userActions = new UserActions(new BookRepository(dbContext));

				userActions.DisplayMenu();
				var userInput = Console.ReadLine();
				switch (userInput)
				{
					case "1":
						userActions.AddBook();
						break;
					case "2":
						userActions.CountBooks();
						break;
					case "3":
						userActions.ListOfBooks();
						break;
					case "4":
						userActions.DeleteBooks();
						break;
				}

				Console.WriteLine("Press any key to exit...");
				Console.ReadKey();
			}

		}


	}
}
