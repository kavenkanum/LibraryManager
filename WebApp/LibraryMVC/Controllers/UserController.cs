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

        public UserController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
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
            var user = _usersRepository.Find(id);
            return View(user);
        }

    }
}