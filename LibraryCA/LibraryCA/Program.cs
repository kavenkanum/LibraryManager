using LibraryDomain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryCA
{

	public enum Command
	{
		Add,
		Count,
		List,
		Delete,
		Edit,
		Unknown
	}

	public class CommandParser
	{
		public Command GetCommandFromUser()
		{
			var command = Console.ReadLine();
			switch (command)
			{
				case "1":
					return Command.Add;
				case "2":
					return Command.Count;
				case "3":
					return Command.List;
				case "4":
					return Command.Delete;
				case "5":
					return Command.Edit;
				default:
					return Command.Unknown;
			}
		}
	}

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
