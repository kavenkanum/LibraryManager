using LibraryMVC.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryMVC.Domain.Commands
{
    public class AddDescriptionCommand : IRequest<bool>
    {
        public int BookId { get; set; }
        public string Description { get; set; }
    }

    public class AddDescriptionCommandHandler : IRequestHandler<AddDescriptionCommand, bool>
    {
        private readonly IBookRepository _bookRepository;
        public AddDescriptionCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public Task<bool> Handle(AddDescriptionCommand request, CancellationToken cancellationToken)
        {
            var book = _bookRepository.Find(request.BookId);
            if (book == null)
            {
                return Task.FromResult(false);
            }
            book.Description = request.Description;
            _bookRepository.Commit();
            return Task.FromResult(true);
        }
    }
}
