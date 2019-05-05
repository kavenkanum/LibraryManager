using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryMVC.Domain.Models;
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

        public IActionResult Activation(int id)
        {
            var user = _usersRepository.Find(id);
            if (ModelState.IsValid)
            {
                _usersRepository.Activation(user);
                _usersRepository.Commit();
            }
            return View(user);
        }

        public IActionResult Deactivation(int userId)
        {
            var user = _usersRepository.Find(userId);
            var borrowedBooks = _borrowedBookRepository.SelectNotReturnedBorrowedBooksByUser(userId);

            //Check if the user returned all of its borrowed books. 
            if (borrowedBooks.Any())
            {
                return RedirectToAction("BooksToReturn", new { userId });
            }

            if (ModelState.IsValid)
            {
                _usersRepository.Deactivation(user);
                _usersRepository.Commit();
            }
            return View(user);
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
                _usersRepository.Commit();
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