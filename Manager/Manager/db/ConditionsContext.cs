using Microsoft.EntityFrameworkCore;

namespace Manager.db
{
    internal class ConditionsContext : DbContext
    {
        public ConditionsContext() => Database.EnsureCreatedAsync();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server = localhost; DataBase = Conditions; Integrated Security = false; User Id = postgres; password = 221100");
        }
        public DbSet<Condition> Conditions { get; set; }
    }
}
