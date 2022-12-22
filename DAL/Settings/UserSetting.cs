
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Settings {
    public class UserSetting : IEntityTypeConfiguration<User> {

        public void Configure(EntityTypeBuilder<User> builder) {
            builder.Property(b => b.Pseudo)
                .IsRequired()
                .HasMaxLength(50);

            //builder.HasIndex("Email").IsUnique();

        }

    }
}
