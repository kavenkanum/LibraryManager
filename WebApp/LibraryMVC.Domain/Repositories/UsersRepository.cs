﻿using LibraryMVC.Domain.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.Linq;


namespace LibraryMVC.Domain.Repositories
{
    public interface IUsersRepository
    {
        void Add(User newPerson);
        void Commit();
        void Delete(User user);
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
        }

        public void Commit()
        {
            _libraryDbContext.SaveChanges();
        }

        public void Delete(User user)
        {
            if (!user.IsActive)
            {
                _libraryDbContext.Remove(user);
            }
            System.Console.WriteLine("Zmień proszę status na NIEAKTYWNY");
        }

        public void Deactivation(User user)
        {
            var userToDeactivate = _libraryDbContext.Users.SingleOrDefault(u => u.ID == user.ID);
            if (userToDeactivate  != null && userToDeactivate.IsActive)
            {
                userToDeactivate.IsActive = false;
                _libraryDbContext.SaveChanges();
            }
        }

        public void Activation(User user)
        {
            var userToActivate = _libraryDbContext.Users.SingleOrDefault(u => u.ID == user.ID);
            if (userToActivate != null && !userToActivate.IsActive)
            {
                userToActivate.IsActive = true;
                _libraryDbContext.SaveChanges();
            }
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
        }
    }
}
