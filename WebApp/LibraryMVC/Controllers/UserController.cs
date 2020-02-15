using System.Collections.Generic;
using System.Linq;
using LibraryMVC.Domain.Entities;
using LibraryMVC.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IBorrowedBookRepository _borrowedBookRepository;

        public UserController(IUsersRepository usersRepository, IBorrowedBookRepository borrowedBookRepository)
        {
            _usersRepository = usersRepository;
            _borrowedBookRepository = borrowedBookRepository;
        }

        public IActionResult Activation(int userId)
        {
            _usersRepository.Activation(userId);
            return View();
        }

        public IActionResult Deactivation(int userId)
        {
            var borrowedBooks = _borrowedBookRepository.SelectNotReturnedBorrowedBooksByUser(userId);

            //Check if the user returned all of its borrowed books. 
            if (borrowedBooks.Any())
            {
                return RedirectToAction("BooksToReturn", new { userId });
            }
            _usersRepository.Deactivation(userId);
            return View();
        }

        public IActionResult BooksToReturn(int userId)
        {
            var user = new UserViewModel
            {
                User = _usersRepository.Find(userId),
                BorrowedBooks = _borrowedBookRepository.SelectNotReturnedBorrowedBooksByUser(userId)
            };

            return View(user);
        }

        public IActionResult Edit(int id)
        {
            var user = _usersRepository.Find(id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                _usersRepository.Edit(user);
            }
            return RedirectToAction("List", "Users");

        }

        public IActionResult View(int id)
        {
            var model = new UserViewModel
            {
                User = _usersRepository.Find(id),
                BorrowedBooks = _borrowedBookRepository.SelectBorrowedBooksByUser(id)
            };

            return View(model);
        }

    }

    public class UserViewModel
    {
        public User User { get; set; }
        public IEnumerable<Book> BorrowedBooks { get; set; }
    }
}