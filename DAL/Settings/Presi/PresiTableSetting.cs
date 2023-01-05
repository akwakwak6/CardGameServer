using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities.Presi;


namespace DAL.Settings.Presi {
    internal class PresiTableSetting : IEntityTypeConfiguration<PresiTable> {

        public void Configure(EntityTypeBuilder<PresiTable> builder) {
            builder.Property(b => b.IsActive).HasDefaultValue(true);
        }

    }
}
