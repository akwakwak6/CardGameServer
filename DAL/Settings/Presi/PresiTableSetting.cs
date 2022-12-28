using Entities.Presi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DAL.Settings.Presi {
    internal class PresiTableSetting : IEntityTypeConfiguration<PresiTable> {

        public void Configure(EntityTypeBuilder<PresiTable> builder) {

            //builder.HasMany(t => t.Players).WithOne();

            builder.Property(b => b.IsActive).HasDefaultValue(true);
        }

    }
}
