using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace LibraryMVC.Domain.Queries
{
    public class GetUsersQueryRow
    {
        public GetUsersQueryRow(int userId, string firstName, string lastName, bool isActive)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            IsActive = isActive;
        }

        public int UserId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public bool IsActive { get; }
    }
    public class GetUsersQuery : IRequest<IEnumerable<GetUsersQueryRow>>
    {
        public GetUsersQuery(string searchString, string sortBy)
        {
            SearchString = searchString;
            SortBy = sortBy;
        }

        public string SearchString { get; }
        public string SortBy { get; }
    }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<GetUsersQueryRow>>
    {
        private readonly LibraryDbContext _libraryDbContext;

        public GetUsersQueryHandler(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
        }

        public Task<IEnumerable<GetUsersQueryRow>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var usersQuery = _libraryDbContext.Users.AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchString))
            {
                usersQuery = usersQuery.Where(u =>
                    (u.FirstName.ToLower().Contains(request.SearchString.ToLower()) ||
                     u.LastName.ToLower().Contains(request.SearchString.ToLower())) ||
                    $"{u.FirstName.ToLower()} {u.LastName.ToLower()}".Contains(request.SearchString.ToLower()));
            }

            switch (request.SortBy)
            {
                case "FirstNameDesc":
                    usersQuery = usersQuery.OrderByDescending(u => u.FirstName);
                    break;
                case "FirstName":
                    usersQuery = usersQuery.OrderBy(u => u.FirstName);
                    break;
                case "LastNameDesc":
                    usersQuery = usersQuery.OrderByDescending(u => u.LastName);
                    break;
                default:
                    usersQuery = usersQuery.OrderBy(u => u.LastName);
                    break;
            }

            IEnumerable<GetUsersQueryRow> results = usersQuery
                .Select(u => new GetUsersQueryRow(u.ID, u.FirstName, u.LastName, u.IsActive))
                .ToList();

            return Task.FromResult(results);
        }
    }
}