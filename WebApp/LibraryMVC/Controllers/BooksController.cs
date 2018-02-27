using LibraryMVC.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookRepository bookRepository;

        public BooksController(IBookRepository _bookRepository)
        {
            bookRepository = _bookRepository;
        }

        public IActionResult List()
        {
            var books = bookRepository.GetBooks();
            return View(books);
        }
    }
}