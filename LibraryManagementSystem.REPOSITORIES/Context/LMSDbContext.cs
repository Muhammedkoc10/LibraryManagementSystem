using LibraryManagementSystem.ENTITIES.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.REPOSITORIES.Context
{
    public class LMSDbContext : DbContext
    {
        public LMSDbContext(DbContextOptions<LMSDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
            public DbSet<Book> Books { get; set; }
            public DbSet<Category> Categories { get; set; }
            public DbSet<RentBook> RentBooks { get; set; }
            public DbSet<User> Users { get; set; }
    }   
}