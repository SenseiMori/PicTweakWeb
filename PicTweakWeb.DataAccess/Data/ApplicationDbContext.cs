using Microsoft.EntityFrameworkCore;
using Models;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Configuration;

namespace DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        //Передача контекста в базу данных
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options)

        {
            



        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            


            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "TestTitle1",
                    Description = "descuyi",
                    ISBN = 12345,
                    Author = "Authorqweqw",
                    ListPrice = 90,
                    ListPrice50 = 80,
                    ListPrice100 = 70,
                    CategoryId = 1,
                    ImageURL = "",
                    
                });
        }

    }
}
