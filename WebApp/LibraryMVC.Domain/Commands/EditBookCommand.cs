using LibraryMVC.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryMVC.Domain.Commands
{
    public class EditBookCommand : IRequest<bool>
    {
        public EditBookCommand(int bookId, string name, string author, int numberAllBooks)
        {
            BookId = bookId;
            Name = name;
            Author = author;
            NumberAllBooks = numberAllBooks;
        }
        public int BookId { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int NumberAllBooks { get; set; }
    }

    public class EditBookCommandHandler : IRequestHandler<EditBookCommand, bool>
    {
        private readonly IBookRepository _bookRepository;
        public EditBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public Task<bool> Handle(EditBookCommand request, CancellationToken cancellationToken)
        {
            var book = _bookRepository.Find(request.BookId);
            if (book == null)
            {
                return Task.FromResult(false);
            }
            var subtractionNewNumberAllBooks = request.NumberAllBooks - book.NumberAllBooks;
            var newNumberAvailableBooks = book.NumberAvailableBooks + subtractionNewNumberAllBooks;
            if (newNumberAvailableBooks < 0)
            {
                return Task.FromResult(false);
            }
            book.Name = request.Name;
            book.Author = request.Author;
            book.NumberAllBooks = request.NumberAllBooks;
            book.NumberAvailableBooks = newNumberAvailableBooks;
            _bookRepository.Commit();
            return Task.FromResult(true);
        }
    }
}
