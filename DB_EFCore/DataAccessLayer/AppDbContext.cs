using DB_EFCore.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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

        public DataTable GetDataTableFromSP(string sp)
        {
            return GetDataTableFromSP(sp, new Dictionary<string, string>());
        }

        public DataTable GetDataTableFromSP(string sp, Dictionary<string, string> parameters)
        {
            try
            {
                using (var command = this.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sp;

                    foreach (var item in parameters)
                    {
                        var param = command.CreateParameter();
                        param.ParameterName = item.Key;
                        param.Value = item.Value;
                        command.Parameters.Add(param);
                    }

                    command.CommandType = CommandType.StoredProcedure;
                    this.Database.OpenConnection();
                    using (var result = command.ExecuteReader())
                    {
                        var dataTable = new DataTable();
                        dataTable.Load(result);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                return new DataTable();
            }
        }
    }
}
