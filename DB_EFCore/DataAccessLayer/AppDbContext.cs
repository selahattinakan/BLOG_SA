using DB_EFCore.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DB_EFCore.DataAccessLayer
{
    public class AppDbContext : DbContext
    {
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Article> Article { get; set; }
        public DbSet<ArticleComment> ArticleComment { get; set; }
        public DbSet<Chat> Chat { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<Setting> Setting { get; set; }
        public DbSet<Subscriber> Subscriber { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Initilazier.Build();
            optionsBuilder.UseSqlServer(Initilazier.Configuration.GetConnectionString("SqlCon"));
        }
    }
}
