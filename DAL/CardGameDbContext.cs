

using DAL.Settings;
using DAL.Settings.Presi;
using Entities;
using Entities.Presi;
using Microsoft.EntityFrameworkCore;

namespace DAL {
    public class CardGameDbContext : DbContext {

        public DbSet<User> Users => Set<User>();
        public DbSet<PresiPlayer> PresiPlayers => Set<PresiPlayer>();
        public DbSet<PresiTable> PresiTables => Set<PresiTable>();


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CardGame;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            new UserSetting().Configure(modelBuilder.Entity<User>());
            new PresiPlayerSetting().Configure(modelBuilder.Entity<PresiPlayer>());
            new PresiTableSetting().Configure(modelBuilder.Entity<PresiTable>());


            //TODO add default users (ADMIN)
            //modelBuilder.Entity<User>().HasData(new User { Id = 1, Pseudo = "ADMIN", Pwd = "ADMIN" });
            //modelBuilder.Entity<User>().HasData(new User { Id = 2, Pseudo = "USER", Pwd = "USER" });

        }

    }
}
