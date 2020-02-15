using System.Threading;
using System.Threading.Tasks;
using LibraryMVC.Domain.Repositories;
using MediatR;

namespace LibraryMVC.Domain.Commands
{
    public class AddImageCommand : IRequest<bool>
    {
        public AddImageCommand(int bookId, string image)
        {
            BookId = bookId;
            Image = image;
        }

        public int BookId { get; }
        public string Image { get; }
    }

    public class AddImageCommandHandler : IRequestHandler<AddImageCommand, bool>
    {
        private readonly IBookRepository _bookRepository;

        public AddImageCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public Task<bool> Handle(AddImageCommand request, CancellationToken cancellationToken)
        {
            var book = _bookRepository.Find(request.BookId);
            if (book == null)
                return Task.FromResult(false);

            book.Image = request.Image;
            _bookRepository.Commit();

            return Task.FromResult(true);
        }
    }
}