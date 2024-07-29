using Microsoft.EntityFrameworkCore;
using WebJaguarPortal.Models;
using WebJaguarPortal.Services;

namespace WebJaguarPortal.Repository
{
    public class ProjectRepository : GenericRepository<Project>
    {
        public ProjectRepository(DbContext context) : base(context)
        {


        }

        public Project? GetByKey(string key)
        {
            var project = dbSet.FirstOrDefault(x => x.Key == key);
            return project;
        }
    }
}
