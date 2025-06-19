using System.Linq.Expressions;
using DataAccess.Data;
using DataAccess.Repository.IRepository;

namespace DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        internal Microsoft.EntityFrameworkCore.DbSet<T> dbSet; 

        private readonly ApplicationDbContext _db;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public void Add(T item)
        {
            dbSet.Add(item);
        }

        public void DeleteRange(IEnumerable<T> items)
        {
            dbSet.RemoveRange(items);
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            if (query == null)
            {
                throw new NotImplementedException();

            }
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return query.ToList();
        }

        public void Remove(T item)
        {
            dbSet.Remove(item);
        }
    }
}
