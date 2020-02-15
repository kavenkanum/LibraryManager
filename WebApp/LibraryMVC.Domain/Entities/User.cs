using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryMVC.Domain.Entities
{
    public enum Status
    {
        Active,
        Inactive,
        Deleted
    }
    public class User
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual List<BorrowedBook> BorrowedBooks { get; set; }
        public Status Status { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
            private set { }
        }
    }
}
