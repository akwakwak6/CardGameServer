

using DAL.Settings;
using Microsoft.EntityFrameworkCore;
using DAL.Settings.Presi;
using Entities.Presi;
using Entities;

namespace DAL {
    public class CardGameDbContext : DbContext {

        public DbSet<User> Users => Set<User>();
        public DbSet<PresiPlayer> PresiPlayers => Set<PresiPlayer>();
        public DbSet<PresiTable> PresiTables => Set<PresiTable>();

        public DbSet<PresiGame> PresiGame => Set<PresiGame>();
        public DbSet<PresiPlayedCards> PresiPlayedCard => Set<PresiPlayedCards>();


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CardGame;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            new UserSetting().Configure(modelBuilder.Entity<User>());
            new PresiTableSetting().Configure(modelBuilder.Entity<PresiTable>());
            new PresiPlayerSetting().Configure(modelBuilder.Entity<PresiPlayer>());
            new PresiGameSetting().Configure(modelBuilder.Entity<PresiGame>());
            new PresiPlayedCardsSetting().Configure(modelBuilder.Entity<PresiPlayedCards>());

        }

    }
}
