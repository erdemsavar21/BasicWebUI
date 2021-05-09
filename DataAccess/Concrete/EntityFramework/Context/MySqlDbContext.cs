using System;
using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework.Context
{
    public class MySqlDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           // optionsBuilder.UseMySql("Server=mysqldemodbserver.mysql.database.azure.com;Port=3306;Database=northwind2;Uid=erdem@mysqldemodbserver;Pwd=Mysql2408!;SslMode=Preferred;");
            optionsBuilder.UseMySql("Server=localhost;Port=3306;Database=northwind2;userid=root;Pwd=Mysql2408!;sslmode=none;AllowPublicKeyRetrieval=true;");
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
    }
}
