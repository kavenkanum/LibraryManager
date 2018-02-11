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
				CommandParser commandParser = new CommandParser();
				var availableCommands = new Dictionary<Command, Action>()
				{
					{Command.Add, userActions.AddBook },
					{Command.Count, userActions.CountBooks },
					{Command.List, userActions.ListOfBooks },
					{Command.Delete, userActions.DeleteBooks },
					{Command.Edit, userActions.EditOrDeleteBook },
					{Command.Unknown, userActions.Exit }
				};
				
				var currentCommand = Command.Unknown;

				do
				{
					userActions.DisplayMenu();
					currentCommand = commandParser.GetCommandFromUser();

					if (availableCommands.ContainsKey(currentCommand))
					{
						availableCommands[currentCommand]();
					}
				} while (currentCommand != Command.Unknown);

				Console.ReadKey();
			}

		}


	}
}
