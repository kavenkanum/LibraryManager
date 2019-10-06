using System.Diagnostics;
using LibraryMVC.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using LibraryMVC.Entities;
using Microsoft.AspNetCore.Authorization;

namespace LibraryMVC.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IBookRepository _bookRepository;

        public HomeController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public IActionResult Index()
        {
            var books = _bookRepository.GetBooks();
            
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}