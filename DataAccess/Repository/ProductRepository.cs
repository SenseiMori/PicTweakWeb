using DataAccess.Data;
using DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TestModels;

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
