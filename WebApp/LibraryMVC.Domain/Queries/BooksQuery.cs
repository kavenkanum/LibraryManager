using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using LibraryMVC.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace LibraryMVC.Domain.Queries
{
    public class BooksQueryRow
    {
        public BooksQueryRow(int bookId, string name, string author, int numberAvailableBooks)
        {
            BookId = bookId;
            Name = name;
            Author = author;
            NumberAvailableBooks = numberAvailableBooks;
        }
        public int BookId { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int NumberAvailableBooks { get; set; }
    }
    public class BooksQuery : IRequest<IEnumerable<BooksQueryRow>>
    {
        public BooksQuery(string searchString)
        {
            SearchString = searchString;
        }
        public string SearchString { get; set; }
    }

    public class BooksQueryHandler : IRequestHandler<BooksQuery, IEnumerable<BooksQueryRow>>
    {
        private readonly LibraryDbContext _libraryDbContext;
        public BooksQueryHandler(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
        }
        public Task<IEnumerable<BooksQueryRow>> Handle(BooksQuery request, CancellationToken cancellationToken)
        {
            var booksQuery = _libraryDbContext.Books.Where(b => b.NumberAvailableBooks > 0);
            if (request.SearchString != null)
            {
                booksQuery = booksQuery.Where(b => b.Name.Contains(request.SearchString) || b.Author.Contains(request.SearchString));
            }
            IEnumerable<BooksQueryRow> results = booksQuery.Select(b => new BooksQueryRow(b.ID, b.Name, b.Author, b.NumberAvailableBooks))
                                    .ToList();

            return Task.FromResult(results);
        }
    }
}
