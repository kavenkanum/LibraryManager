using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryMVC.Domain.Models;
using LibraryMVC.Domain.Repositories;


namespace LibraryMVC.Controllers
{
    public class BorrowBookController : Controller
    {

        private readonly IUsersRepository _usersRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IBorrowedBookRepository _borrowedBookRepository;

        public BorrowBookController(IUsersRepository usersRepository, IBookRepository bookRepository, IBorrowedBookRepository borrowedBookRepository)
        {
            _usersRepository = usersRepository;
            _bookRepository = bookRepository;
            _borrowedBookRepository = borrowedBookRepository;
        }

        public IActionResult SelectUser(string searchString, string sortBy)
        {
            var users = _usersRepository.GetUsers();
            
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => (u.FirstName.ToLower().Contains(searchString.ToLower()) || u.LastName.ToLower().Contains(searchString.ToLower())) || $"{u.FirstName.ToLower()} {u.LastName.ToLower()}".Contains(searchString.ToLower()));
            }

            ViewBag.LastNameSortParm = string.IsNullOrEmpty(sortBy) ? "LastNameDesc" : "";
            ViewBag.FirstNameSortParm = sortBy == "FirstName" ? "FirstNameDesc" : "FirstName";

            switch (sortBy)
            {
                case "FirstNameDesc":
                    users = users.OrderByDescending(u => u.FirstName);
                    break;
                case "FirstName":
                    users = users.OrderBy(u => u.FirstName);
                    break;
                case "LastNameDesc":
                    users = users.OrderByDescending(u => u.LastName);
                    break;
                default:
                    users = users.OrderBy(u => u.LastName);
                    break;
            }
            return View(users.ToList());
        }

        

        public IActionResult SelectBorrowingUser()
        {
            var model = _borrowedBookRepository.GetUsersWithBorrowedBooks();
            
            return View(model);
        }

        public IActionResult SelectBook(int forUserId, string searchString)
        {
            var model = new SelectBookViewModel
            {
                AvailableBooks = _bookRepository.GetAvailableBooks(),
                UserId = forUserId
            };

            if (!String.IsNullOrEmpty(searchString))
            {
                model.AvailableBooks = model.AvailableBooks.Where(b => b.Name.ToLower().Contains(searchString.ToLower()) || b.Author.ToLower().Contains(searchString.ToLower()));
            }

            return View(model);
        }

        public IActionResult BorrowBook(int bookId, int forUserId)
        {
            _borrowedBookRepository.Borrow(bookId, forUserId);


            var model = new BorrowBookViewModel
            {
                UserId = forUserId,
                Book = _bookRepository.Find(bookId)
            };
            return View(model);
        }

        public IActionResult ReturnBook(int borrowedBookId)
        {
            var bookId = _borrowedBookRepository.Find(borrowedBookId);
            var book = _bookRepository.Find(bookId);

            return View(book);
        }

        [HttpPost, ActionName("ReturnBook")]
        [ValidateAntiForgeryToken]
        public IActionResult ReturnBookConfirmed(int borrowedBookId)
        {
            _borrowedBookRepository.Return(borrowedBookId);

            return RedirectToAction("SelectBorrowingUser", "BorrowBook");
        }

        public IActionResult BorrowingHistory()
        {
            var model = _borrowedBookRepository.ListOfAllBorrowedBooks();
            return View(model);
        }
    }

    public class SelectBookViewModel
    {
        public int UserId { get; set; }
        public IEnumerable<Book> AvailableBooks { get; set; }
    }

    public class BorrowBookViewModel
    {
        public int UserId { get; set; }
        public Book Book { get; set; }
    }
}