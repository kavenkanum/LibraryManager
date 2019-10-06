using LibraryMVC.Domain.Entities;
using LibraryMVC.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace LibraryMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookRepository bookRepository;
        private readonly IUsersRepository userRepository;

        public BooksController(IBookRepository _bookRepository, IUsersRepository _userRepository)
        {
            bookRepository = _bookRepository;
            userRepository = _userRepository;
        }

        public IActionResult List()
        {
            var books = bookRepository.GetBooks();
            return View(books);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Book book)
        {
            bookRepository.Add(book);
            bookRepository.Commit();
            return RedirectToAction("List");
        }

        public IActionResult Delete()
        {
            var books = bookRepository.GetBooks();
            return View(books);
        }




        
        
    }
}