using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models;

namespace DataAccess.Repository
{
    //Поскольку класс наследуется от интерфейса, который включает в себя два метода + методы из родительского интерфейса,
    // то здесь не нужно писать их, ведь они реализованы в базовом классе

    public class ProductRepository :Repository <Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
 
        public void Update(Product obj)
        {
            _db.Products.Update(obj);
        }
    }
}
