using Alerting.Infrastructure.Data.DbContexts.Entities;
using Microsoft.EntityFrameworkCore;

namespace Alerting.Infrastructure.Data.DbContexts
{
    public class AlertingDbContext : DbContext, IUnitOfWork
    {
        public DbSet<ContactPerson> ContactPersons { get; set; }

        public AlertingDbContext(DbContextOptions<AlertingDbContext> options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AlertingDbContext).Assembly);
        }
    }
}