using Entities.Presi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DAL.Settings.Presi {
    internal class PresiGameSetting : IEntityTypeConfiguration<PresiGame> {
        public void Configure(EntityTypeBuilder<PresiGame> builder) {
            builder.Property(g => g.StartTime).HasDefaultValue(DateTime.Now);
        }
    }
}
