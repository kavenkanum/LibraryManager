using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryMVC.Domain.Models;
using LibraryMVC.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsersRepository usersRepository;
        public LoginController(IUsersRepository _usersRepository)
        {
            usersRepository = _usersRepository;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            usersRepository.Add(user);
            usersRepository.Commit();
            return RedirectToAction("List");
        }
    }
}