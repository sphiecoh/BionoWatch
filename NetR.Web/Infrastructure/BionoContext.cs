using Microsoft.EntityFrameworkCore;
using NetR.Web.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetR.Web.Infrastructure
{
    public class BionoContext : DbContext
    {
        public BionoContext(DbContextOptions<BionoContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationGroup>().HasKey(sc => new { sc.GroupId, sc.EmailNotificationId });
            modelBuilder.Entity<ServiceGroup>().HasKey(sc => new { sc.GroupId, sc.ServiceId });
        }
        public DbSet<ServiceConfiguration> ServiceConfiguration { get; set; }
        public DbSet<Configuration> Configuration { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<NotificationGroup> NotificationGroups { get; set; }
        public DbSet<ServiceGroup> ServiceGroups { get; set; }
        public DbSet<EmailNotification> EmailNotification { get; set; }
    }
}
