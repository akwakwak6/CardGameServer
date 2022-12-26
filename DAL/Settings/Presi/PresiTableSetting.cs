using Entities;
using Entities.Presi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Settings.Presi {
    internal class PresiTableSetting : IEntityTypeConfiguration<PresiTable> {

        public void Configure(EntityTypeBuilder<PresiTable> builder) {

        }

    }
}
