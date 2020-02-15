using LibraryMVC.Domain.Entities;
using LibraryMVC.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
            var users = usersRepository.GetUsers();
            return View(users);
        }

        public IActionResult List()
        {
            var users = usersRepository.GetUsers();
            return View(users);
        }

        [HttpPost]
        public IActionResult Add(AddUserModel user)
        {
            if (ModelState.IsValid)
            {
                usersRepository.Add(user.FirstName, user.LastName);
                return RedirectToAction("List");
            }
            return View(user);
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
    public class AddUserModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}