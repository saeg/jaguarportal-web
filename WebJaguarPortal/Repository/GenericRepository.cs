using Microsoft.EntityFrameworkCore;
using WebJaguarPortal.Repository.Interfaces;

namespace WebJaguarPortal.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly DbSet<T> dbSet;
        private readonly DbContext context;

        public GenericRepository(DbContext context)
        {
            dbSet = context.Set<T>();
            this.context = context;
        }

        public virtual void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            dbSet.Update(entity);
        }

        public virtual void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public virtual T? GetById(long id)
        {
            return dbSet.Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }
        public virtual long Count()
        {
            return dbSet.Count();
        }

        public virtual void SaveChanges()
        {
            context.SaveChanges();
        }
    }

}
