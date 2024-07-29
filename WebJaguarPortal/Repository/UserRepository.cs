using Microsoft.EntityFrameworkCore;
using WebJaguarPortal.Models;
using WebJaguarPortal.Services;

namespace WebJaguarPortal.Repository
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(DbContext context) : base(context)
        {


        }

        public User? GetByUsernameAndPassword(string username, string password)
        {
            User? user = dbSet.FirstOrDefault(x => x.Username == username || x.Email == username);

            if (user != null && Util.VerifyHash(password, user.Password))
                return user;
            else
                return null;
        }

        public User? GetByUsername(string username)
        {
            return dbSet.FirstOrDefault(x => x.Username == username);
        }

        internal User? GetByClientIdAndClientSecret(string clientId, string clientSecret)
        {
            User? user = dbSet.FirstOrDefault(x => x.ClientId == clientId);

            if (user != null && Util.VerifyHash(clientSecret, user.ClientSecret))
                return user;
            else
                return null;
        }

        internal User? GetByEmail(string email)
        {
            User? user = dbSet.FirstOrDefault(x => x.Email == email);
            return user;
        }

        internal User? GetByUsernameOrEmail(string userNameOrEmail)
        {
            User? user = dbSet.FirstOrDefault(x => x.Email == userNameOrEmail || x.Username == userNameOrEmail);
            return user;
        }

        internal bool VerifyUniqueEmail(string email, long? id = null)
        {
            int count = 0;

            if (id == null)
                count = dbSet.Count(x => x.Email == email);
            else
                count = dbSet.Count(x => x.Email == email && x.Id != id);

            return count == 0;
        }

        internal bool VerifyUniqueUsername(string username)
        {
            int count = dbSet.Count(x => x.Username == username);

            return count == 0;
        }

        internal bool VerifyUniqueAdmin(long id)
        {
            if (dbSet.Any(x => x.Roles != null && x.Roles.IsAdmin && x.Id == id))
            {
                if (!dbSet.Any(x => x.Roles != null && x.Roles.IsAdmin && x.Id != id))
                    return false;
                else
                    return true;
            }
            else
            {
                return true;
            }
        }
        internal bool VerifyDeleteUniqueAdmin(long id)
        {
            if (dbSet.Any(x => x.Roles != null && x.Roles.IsAdmin && x.Id == id))
            {
                if (dbSet.Count(x => x.Roles != null && x.Roles.IsAdmin) == 1)
                    return false;
                else
                    return true;
            }
            else
            {
                return true;
            }
        }
    }
}
