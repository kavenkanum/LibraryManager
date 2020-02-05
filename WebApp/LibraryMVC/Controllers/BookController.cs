using System;
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
                return View(book);
            }
            return View("Error");
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                _bookRepository.Edit(book);
                return RedirectToAction("List", "Books");
            }
            return View(book);
        }
        [HttpGet]
        public IActionResult AddDescription(int id)
        {
            return View(new AddDescriptionCommand()
            {
                BookId = id
            });

            //Book book = _bookRepository.Find(id);
            //return View(book);
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

    
    public class AddImageModel
    {
        public int Id { get; set; }
        public IFormFile Image { get; set; }
    }

}