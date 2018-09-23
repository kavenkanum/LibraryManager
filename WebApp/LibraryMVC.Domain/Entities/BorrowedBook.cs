using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryMVC.Domain.Models
{
    public class BorrowedBook
    {
        public int ID { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
