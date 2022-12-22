

using DAL.Settings;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL {
    public class CardGameDbContext : DbContext {

        public DbSet<User> Users => Set<User>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CardGame;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            new UserSetting().Configure(modelBuilder.Entity<User>());

            modelBuilder.Entity<User>().HasData(new User { Id = 1, Pseudo = "ADMIN", Pwd = "ADMIN" });
            modelBuilder.Entity<User>().HasData(new User { Id = 2, Pseudo = "USER", Pwd = "USER" });

        }

    }
}
