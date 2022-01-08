using Alerting.Infrastructure.Data.DbContexts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alerting.Infrastructure.Data.DbContexts.EntityTypeConfigurations
{
    public class ContactPersonEntityTypeConfiguration : IEntityTypeConfiguration<ContactPerson>
    {
        public void Configure(EntityTypeBuilder<ContactPerson> builder)
        {
            builder.ToTable("ContactPersons");
        }
    }
}