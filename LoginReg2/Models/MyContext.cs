using Microsoft.EntityFrameworkCore;
namespace LoginReg2.Models
{
    public class MyContext:DbContext
    {
        public MyContext (DbContextOptions options) : base (options) { }
        public DbSet<User> Users { get; set; }

    }
}