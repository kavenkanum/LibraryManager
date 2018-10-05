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

        public IActionResult Delete(int id)
        {
            var user = _usersRepository.Find(id);
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCondfirmed(User user)
        {
            _usersRepository.Delete(user);
            _usersRepository.Commit();
            return RedirectToAction("List", "Users");

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