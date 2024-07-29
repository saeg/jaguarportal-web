using Microsoft.EntityFrameworkCore;
using WebJaguarPortal.Models;
using WebJaguarPortal.Services;

namespace WebJaguarPortal.Repository
{
    public class RenewPasswordRepository : GenericRepository<RenewPassword>
    {
        public RenewPasswordRepository(DbContext context) : base(context)
        {


        }
        
        internal RenewPassword? GetByKey(string key)
        {
            RenewPassword? request = dbSet.FirstOrDefault(x => x.Key == key);
            return request;
        }        
    }
}
