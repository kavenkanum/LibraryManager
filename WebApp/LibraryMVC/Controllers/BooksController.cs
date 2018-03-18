using LibraryMVC.Domain.Models;
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

        public IActionResult Edit()
        {
            return View();
        }
        
    }
}