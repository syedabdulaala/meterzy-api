using Meterzy.Entity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Meterzy.Data.Config
{
    internal abstract class TableConfig<T> where T : Table
    {
        protected EntityTypeBuilder<T> Entity { get; }

        internal TableConfig(ModelBuilder modelBuilder)
        {
            Entity = modelBuilder.Entity<T>();
            Entity.Property(x => x.Deleted).IsRequired(true);
            Entity.Property(x => x.CreatedOn).IsRequired(true);
            Entity.Property(x => x.LastModifiedOn).IsRequired(false);
        }
    }
}
