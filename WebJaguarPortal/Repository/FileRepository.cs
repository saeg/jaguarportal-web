using Microsoft.EntityFrameworkCore;
using WebJaguarPortal.Models;
using WebJaguarPortal.Services;

namespace WebJaguarPortal.Repository
{
    public class FileRepository : GenericRepository<FileAnalysis>
    {
        public FileRepository(DbContext context) : base(context)
        {


        }

        public bool GetIdFileByHash(string hash, out long? id)
        {
            var file = dbSet.FirstOrDefault(x => x.Hash == hash);

            if (file == null)
            {
                id = null;
                return false;
            }
            else
            {
                id = file.Id;
                return true;
            }
        }
    }
}
