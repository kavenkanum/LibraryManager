using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDomain
{
	public class Person
	{
		public int PersonID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public Book BorrowedBooks { get; set; }
	}
}
