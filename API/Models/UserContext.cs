using Microsoft.EntityFrameworkCore;


namespace API.Models
{
    public class UserContext:DbContext
    {
        public UserContext(DbContextOptions dbContextOptions)
       : base(dbContextOptions)
        {
        }
        public DbSet<Login> Login { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Login>().HasData(new Login
            {
                Id = 1,
                Nick = "rgatilanov",
                Password = "96cae35ce8a9b0244178bf28e4966c2ce1b8385723a96a6b838858cdd6ca0a1e"
            });
        }
    }
}
