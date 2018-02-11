using LibraryDomain;
using System;

namespace LibraryCA
{
	public class UserActions
	{
		DbBook dbContext = new DbBook();
		private readonly BookRepository _bookRepository;
	
		public UserActions(BookRepository bookRepository)
		{
			_bookRepository = bookRepository;
		}

		public void DisplayMenu()
		{
			Console.WriteLine("What do you want to do?");
			Console.WriteLine("1. Add book");
			Console.WriteLine("2. Write a number of books");
			Console.WriteLine("3. Write all books");
			Console.WriteLine("4. Delete all books");

		}

		public void AddBook()
		{
			Console.WriteLine("Enter a name for your book");
			var name = Console.ReadLine();
			Console.WriteLine("Enter an author's  name:");
			var author = Console.ReadLine();
			var book = new Book { Name = name , Author = author};
			_bookRepository.Add(book);
			_bookRepository.Commit();
		}

		public void CountBooks()
		{
			Console.WriteLine("Number of books in repository");
			var numberOfBooks = _bookRepository.CountBooks();
			Console.WriteLine(numberOfBooks);
		}

		public void ListOfBooks()
		{
			Console.WriteLine("List of your books:");
			foreach (var book in _bookRepository.GetBooks())
			{
				Console.WriteLine(book.Name);
			}			
		}

		public void DeleteBooks()
		{
			Console.WriteLine("Your list of books will be removed.");

			foreach (var book in _bookRepository.GetBooks())
			{
				_bookRepository.Delete(book);
			}
			_bookRepository.Commit();
		}
	}
}
