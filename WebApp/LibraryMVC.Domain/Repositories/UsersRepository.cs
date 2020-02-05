﻿using LibraryMVC.Domain.Entities;
using System.Collections.Generic;
using System.Linq;


namespace LibraryMVC.Domain.Repositories
{
    public interface IUsersRepository
    {
        void Add(User newUser);
        void Deactivation(User user);
        void Activation(User user);
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

        public void Add(User newUser)
        {
            _libraryDbContext.Users.Add(newUser);
            _libraryDbContext.SaveChanges();
        }

        public void Deactivation(User user)
        {
            var userToDeactivate = _libraryDbContext.Users.SingleOrDefault(u => u.ID == user.ID);
            if (userToDeactivate  != null && userToDeactivate.IsActive)
            {
                userToDeactivate.IsActive = false;
            }
            _libraryDbContext.SaveChanges();
        }

        public void Activation(User user)
        {
            var userToActivate = _libraryDbContext.Users.SingleOrDefault(u => u.ID == user.ID);
            if (userToActivate != null && !userToActivate.IsActive)
            {
                userToActivate.IsActive = true;
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
