using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryMVC.Domain.Models
{
    public class Book
    {
        public int ID { get; set; }

        //[Required(ErrorMessage = "Please enter the user's first name.")]
        //[StringLength(5, ErrorMessage = "The First Name must be less than {1} characters.")]
        //[Display(Name = "First Name:")]

        public string Name { get; set; }
        public string Author { get; set; }
        public int NumberAvailableBooks { get; set; }
        public IEnumerable<BorrowedBook> BorrowingUsers { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
