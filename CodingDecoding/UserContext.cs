using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingDecoding
{
    public class UserContext:DbContext
    {
        public UserContext():base("UsersDb")
        {

        }
        public DbSet<User> Users { get; set; }
    }
}
