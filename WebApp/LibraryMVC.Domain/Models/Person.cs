namespace LibraryMVC.Domain.Models
{
	public class Person
	{
		public int ID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public Book BorrowedBooks { get; set; }
	}
}
