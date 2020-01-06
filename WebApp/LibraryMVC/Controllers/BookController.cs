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
            this._bookRepository = bookRepository;
            _imageReader = imageReader;
            _mediator = mediator;
        }

        public IActionResult View(int id)
        {
            //TODO: If book is null => NotFound()
            var book = _bookRepository.Find(id);
            return View(book);
        }

        public IActionResult Delete(int id)
        {
            //TODO: What if someone has borrowed this book?
            //TODO: If book is null => NotFound()
            var book = _bookRepository.Find(id);
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //TODO: Delete(int id)
            //TODO: SaveChanges should be executed inside repository, not in controller
            Book book = _bookRepository.Find(id);
            _bookRepository.Delete(book);
            _bookRepository.Commit();
            return RedirectToAction("List", "Books");
        }

        public IActionResult Edit(int id)
        {
            //TODO: If book is null => NotFound()
            Book book = _bookRepository.Find(id);
            return View(book);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Book book)
        {
            //TODO: SaveChanges should be executed inside repository, not in controller
            if (ModelState.IsValid)
            {
                _bookRepository.Edit(book);
                _bookRepository.Commit();
                return RedirectToAction("List", "Books");
            }
            return View(book);
        }

        public IActionResult AddDescription(int id)
        {
            Book book = _bookRepository.Find(id);
            return View(book);
        }

        [HttpPost]
        public IActionResult AddDescription(AddDescriptionModel model)
        {
            _bookRepository.AddDescritpion(model.Id, model.Description);
            return RedirectToAction("View", new { model.Id });
        }

        //TODO: Refactor
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