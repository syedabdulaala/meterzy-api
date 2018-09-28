using Meterzy.Entity.Data;
using Microsoft.EntityFrameworkCore;

namespace Meterzy.Data.Config
{
    internal sealed class AppUserConfig : TableConfig<AppUser>
    {
        internal AppUserConfig(ModelBuilder modelBuilder) : base(modelBuilder)
        {
            Entity.HasKey(x => x.Id);
            Entity.Property(x => x.DisplayName).IsRequired(true);
            Entity.Property(x => x.EmailHash).IsRequired(true);
            Entity.Property(x => x.PasswordHash).IsRequired(true);
            Entity.Property(x => x.Status).IsRequired(true);
        }
    }
}
