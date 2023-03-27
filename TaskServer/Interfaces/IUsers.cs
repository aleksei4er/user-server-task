using System;
using TaskServer.Entities;

namespace TaskServer.Interfaces
{
	public interface IUsers
	{
		public IEnumerable<User> AllUsers();

        public User Find(int id);

        public void Create(User user);

        public User Remove(int id);

        public User SetStatus(int id, UserStatus status);
    }
}
