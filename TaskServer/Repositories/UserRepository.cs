using System;
using TaskServer.Interfaces;
using TaskServer.Entities;
using System.ComponentModel;

namespace TaskServer.Repositories
{
	public class UserRepository: IUsers
	{
        private readonly Context _context;

        public UserRepository(Context context)
        {
            _context = context;
        }

        IEnumerable<User> IUsers.AllUsers() => _context.Users;

        User IUsers.Find(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            return user;
        }

        void IUsers.Create(User user)
        {
            _context.Add(user);
            _context.SaveChanges();
        }

        User IUsers.Remove(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return user;
        }

        User IUsers.SetStatus(int id, UserStatus status)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.Status = status;
            _context.SaveChanges();

            return user;
        }
    }
}
