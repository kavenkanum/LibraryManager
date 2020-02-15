using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using LibraryMVC.Domain.Commands;
using LibraryMVC.Domain.Entities;
using LibraryMVC.Domain.Repositories;
using LibraryMVC.Utils;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryMVC.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IImageReader _imageReader;
        private readonly IMediator _mediator;

        public BookController(IBookRepository bookRepository, IImageReader imageReader, IMediator mediator)
        {
            _bookRepository = bookRepository;
            _imageReader = imageReader;
            _mediator = mediator;
        }

        public IActionResult View(int id)
        {
            var book = _bookRepository.Find(id);
            if (book != null)
            {
                return View(book);
            }
            return View("Error");
        }

        public IActionResult Delete(int id)
        {
            var book = _bookRepository.Find(id);
            if (book != null)
            {
                return View(book);
            }
            return View("Error");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var result = _bookRepository.Delete(id);
            return result ? RedirectToAction("List", "Books") : RedirectToAction("Error");
        }

        public IActionResult Edit(int id)
        {
            //TODO: If book is null => NotFound()
            Book book = _bookRepository.Find(id);
            if (book != null)
            {
                return View(new EditBookModel()
                {
                    BookId = id,
                    Name = book.Name,
                    Author = book.Author,
                    NumberAllBooks = book.NumberAllBooks
                });

            }
            return View("Error");
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditBookModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(new EditBookCommand(model.BookId, model.Name, model.Author, model.NumberAllBooks));
                return result ? RedirectToAction("List", "Books") : RedirectToAction("Error");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult AddDescription(int id)
        {
            Book book = _bookRepository.Find(id);
            return View(new AddDescriptionCommand()
            {
                BookId = id,
                Description = book.Description
            });

        }

        [HttpPost]
        public async Task<IActionResult> AddDescription(AddDescriptionCommand model)
        {
            var result = await _mediator.Send(model);
            return result ? RedirectToAction("View", new {id = model.BookId }) : RedirectToAction("Error");
        }

        [HttpPost]
        public async Task<IActionResult> AddImage(AddImageModel model)
        {
            var imageInBase64 = await _imageReader.ConvertToBase64Async(model.Image);
            var result  = await _mediator.Send(new AddImageCommand(model.Id, imageInBase64));
            return result ? RedirectToAction("View", new {model.Id}) : RedirectToAction("Error");
        }

        [HttpGet]
        public IActionResult AddImage(int id)
        {
            return View(new AddImageModel()
            {
                Id = id
            });
        }

        public IActionResult Error()
        {
            return View();
        }

    }
    public class EditBookModel
    {
        public int BookId { get; set; }
        [Required(ErrorMessage = "Name of the book is required")]
        public string Name { get; set; }
        [Required]
        public string Author { get; set; }
        public int NumberAllBooks { get; set; }
    }
    public class AddImageModel
    {
        public int Id { get; set; }
        public IFormFile Image { get; set; }
    }

}