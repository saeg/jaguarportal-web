namespace WebJaguarPortal.Repository.Interfaces
{
    public interface IRepository<T>
    {      
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        T? GetById(long id);
        IEnumerable<T> GetAll();
        void SaveChanges();
        long Count();
    }

}
