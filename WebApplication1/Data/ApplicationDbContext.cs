using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
namespace WebApplication1.Data
{
    public class ApplicationDbContext : DbContext
    {
        //Передача контекста в базу данных
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options)

        {
            



        }

        //Создает в базе данных таблицу Categories и сам пишет код на создание таблицы, который можно посмотреть в папке Data
        //Но, чтобы он применился в таблице, неоходимо отправить запрос на проверку в консоли update-database
        public DbSet <Category> Categories{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Mystic", DisplayOrder = 2 },
                new Category { Id = 3, Name = "History", DisplayOrder =3 },
                new Category { Id = 4, Name = "SciFi", DisplayOrder =  4 }
                );
        }

    }
}
