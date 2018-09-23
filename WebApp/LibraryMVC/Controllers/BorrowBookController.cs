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

        public IActionResult SelectUser()
        {
            var users = _usersRepository.GetUsers();
            return View(users);
        }

        public IActionResult SelectBorrowingUser()
        {
            var users = _borrowedBookRepository.GetBorrowingUsers();
            return View(users);
        }

        public IActionResult SelectBook(int forUserId)
        {
            var model = new SelectBookViewModel
            {
                AvailableBooks = _bookRepository.GetAvailableBooks(),
                UserId = forUserId
            };
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