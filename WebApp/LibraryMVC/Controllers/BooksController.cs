using LibraryMVC.Domain.Entities;
using LibraryMVC.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LibraryMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUsersRepository _userRepository;

        public BooksController(IBookRepository bookRepository, IUsersRepository userRepository)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }

        public IActionResult List()
        {
            var books = _bookRepository.GetBooks();
            return View(books);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddBookModel book)
        {
            if (ModelState.IsValid)
            {
                _bookRepository.Add(book.Name, book.Author, book.NumberAllBooks);
                return RedirectToAction("List");
            }
            return View(book);
        }

        public IActionResult Delete()
        {
            var books = _bookRepository.GetBooks();
            return View(books);
        }
       
    }

    public class AddBookModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public int NumberAllBooks { get; set; }
    }
}