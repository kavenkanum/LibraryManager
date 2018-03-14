using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryMVC.Domain.Models;
using LibraryMVC.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryMVC.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository bookRepository;
        public BookController(IBookRepository _bookRepository)
        {
            bookRepository = _bookRepository;
        }
       

        public IActionResult View(int id)
        {
            return View();
        }

        public IActionResult Delete(int id)
        {
            Book book = new Book();
            book = bookRepository.Find(id);
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = bookRepository.Find(id);
            bookRepository.Delete(book);
            bookRepository.Commit();
            return RedirectToAction("Delete", "Books");
        }
    }
}