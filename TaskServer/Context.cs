using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TaskServer.Entities;

namespace TaskServer
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }

        public Context(DbContextOptions<Context> options): base(options)
        {
        }

        public void Seed()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();

            var user1 = new User()
            {
                Name = "Aleksei",
                Status = UserStatus.Active
            };

            var user2 = new User()
            {
                Name = "Ivan",
                Status = UserStatus.Deleted
            };

            Users.AddRange(new User[] { user1, user2 });

            SaveChanges();
        }

    }
}

