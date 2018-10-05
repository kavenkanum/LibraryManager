using System.Collections.Generic;

namespace LibraryMVC.Domain.Models
{
	public class User
	{
		public int ID { get; set; }
        public string NickName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
        public virtual List<BorrowedBook> BorrowedBooks { get; set; }
        public bool IsActive { get; set; }
	}
}
