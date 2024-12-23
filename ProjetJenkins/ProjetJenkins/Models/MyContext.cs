using Microsoft.EntityFrameworkCore;

namespace ProjetJenkins.Models
{
    public class MyContext :DbContext
    {
        public DbSet<Client> Clients { get; set; }
        
        public MyContext(DbContextOptions<MyContext> opt): base(opt) //Constructeur
        { }
    }
}
