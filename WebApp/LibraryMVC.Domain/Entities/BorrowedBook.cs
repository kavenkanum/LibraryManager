using System;

namespace LibraryMVC.Domain.Entities
{
    public class BorrowedBook
    {
        public int ID { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime DateOfBorrow { get; set; }
        public DateTime? DateOfReturn { get; set; }
    }
}
