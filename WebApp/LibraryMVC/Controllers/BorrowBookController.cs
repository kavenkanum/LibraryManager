using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryMVC.Domain.Entities;
using LibraryMVC.Domain.Queries;
using LibraryMVC.Domain.Repositories;
using MediatR;


namespace LibraryMVC.Controllers
{
    public class BorrowBookController : Controller
    {

        private readonly IUsersRepository _usersRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IBorrowedBookRepository _borrowedBookRepository;
        private readonly IMediator _mediator;

        public BorrowBookController(IUsersRepository usersRepository, IBookRepository bookRepository, IBorrowedBookRepository borrowedBookRepository, IMediator mediator)
        {
            _usersRepository = usersRepository;
            _bookRepository = bookRepository;
            _borrowedBookRepository = borrowedBookRepository;
            _mediator = mediator;
        }

        //TODO: Refactor
        public async Task<IActionResult> SelectUser(string searchString, string sortBy)
        {
            ViewBag.LastNameSortParm = string.IsNullOrEmpty(sortBy) ? "LastNameDesc" : "";
            ViewBag.FirstNameSortParm = sortBy == "FirstName" ? "FirstNameDesc" : "FirstName";

            var users = await _mediator.Send(new GetUsersQuery(searchString, sortBy));

            return View(users);
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