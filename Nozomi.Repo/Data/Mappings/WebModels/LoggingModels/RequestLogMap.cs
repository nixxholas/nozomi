using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.WebModels.LoggingModels;

namespace Nozomi.Repo.Data.Mappings.WebModels.LoggingModels
{
    public class RequestLogMap
    {
        public RequestLogMap(EntityTypeBuilder<RequestLog> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(rl => rl.Id).HasName("RequestLog_PK_Id");
            entityTypeBuilder.Property(rl => rl.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(rl => rl.Type).IsRequired();
            entityTypeBuilder.Property(rl => rl.RawPayload).IsRequired(false);

            entityTypeBuilder.HasOne(rl => rl.Request).WithMany(r => r.RequestLogs)
                .HasForeignKey(rl => rl.RequestId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}