using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LibraryMVC.Domain.Models;
using LibraryMVC.Domain.Repositories;
using Microsoft.AspNetCore.Http;
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
            var book = bookRepository.Find(id);
            return View(book);
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
            return RedirectToAction("List", "Books");
        }

        public IActionResult Edit(int id)
        {
            Book book = bookRepository.Find(id);
            return View(book);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                bookRepository.Edit(book);
                bookRepository.Commit();
                return RedirectToAction("List", "Books");
            }
            return View(book);
        }

        public IActionResult AddDescription(int id)
        {
            Book book = bookRepository.Find(id);
            return View(book);
        }

        [HttpPost]
        public IActionResult AddDescription(AddDescriptionModel model)
        {

            bookRepository.AddDescritpion(model.Id, model.Description);
            return RedirectToAction("View", new { model.Id });
        }

        [HttpPost]
        public async Task<IActionResult> AddImage(AddImageModel model)
        {
            var book = bookRepository.Find(model.Id);
            using (var memoryStream = new MemoryStream())
            {
                await model.Image.CopyToAsync(memoryStream);
                byte[] imageArray = memoryStream.ToArray();
                book.Image = Convert.ToBase64String(imageArray);
            }

            bookRepository.Commit();
            return RedirectToAction("View", new { model.Id });
        }

        [HttpGet]
        public IActionResult AddImage(int id)
        {
            return View(new AddImageModel()
            {
                Id = id
            });
        }

    }

    
    public class AddDescriptionModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }

    public class AddImageModel
    {
        public int Id { get; set; }
        public IFormFile Image { get; set; }
    }

}