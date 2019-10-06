using System.Collections.Generic;

namespace LibraryMVC.Domain.Entities
{
    public class User
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual List<BorrowedBook> BorrowedBooks { get; set; }
        public bool IsActive { get; set; }
	}
}
