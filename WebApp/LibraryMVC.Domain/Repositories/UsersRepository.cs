using LibraryMVC.Domain.Entities;
using System.Collections.Generic;
using System.Linq;


namespace LibraryMVC.Domain.Repositories
{
    public interface IUsersRepository
    {
        void Add(string firstName, string lastName);
        void Deactivation(int userId);
        void Activation(int userId);
        IEnumerable<User> GetUsers();
        User Find(int ID);
        void Edit(User user);

    }

	public class UsersRepository : IUsersRepository
	{
        private readonly LibraryDbContext _libraryDbContext;

        public UsersRepository(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
        }

        public void Add(string firstName, string lastName)
        {
            User newUser = new User() { FirstName = firstName, LastName = lastName };
            newUser.Status = Status.Inactive;
            _libraryDbContext.Users.Add(newUser);
            _libraryDbContext.SaveChanges();
        }

        public void Deactivation(int userId)
        {
            var userToDeactivate = _libraryDbContext.Users.SingleOrDefault(u => u.ID == userId);
            if (userToDeactivate  != null && userToDeactivate.Status == Status.Active)
            {
                userToDeactivate.Status = Status.Inactive;
            }
            _libraryDbContext.SaveChanges();
        }

        public void Activation(int userId)
        {
            var userToActivate = _libraryDbContext.Users.SingleOrDefault(u => u.ID == userId);
            if (userToActivate != null && userToActivate.Status != Status.Active)
            {
                userToActivate.Status = Status.Active;
            }
            _libraryDbContext.SaveChanges();
        }

        public IEnumerable<User> GetUsers()
        {
            return _libraryDbContext.Users.ToList();
        }
        
        public User Find(int ID)
        {
            var user = _libraryDbContext.Users.Find(ID);
            return user;
        }

        public void Edit(User user)
        {
            _libraryDbContext.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _libraryDbContext.SaveChanges();
        }
    }
}
