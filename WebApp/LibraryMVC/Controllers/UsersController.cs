using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryMVC.Domain.Models;
using LibraryMVC.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryMVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersRepository usersRepository;

        public UsersController(IUsersRepository _usersRepository)
        {
            usersRepository = _usersRepository;
        }

        public IActionResult Activation()
        {
            return View();
        }

        public IActionResult List()
        {
            var users = usersRepository.GetUsers();
            return View(users);
        }

        [HttpPost]
        public IActionResult Add(User user)
        {
            usersRepository.Add(user);
            usersRepository.Commit();
            return RedirectToAction("List");
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult BorrowingUser()
        {
            return View();
        }
    }
}