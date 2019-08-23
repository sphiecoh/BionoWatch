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
        public DbSet<ServiceConfiguration> ServiceConfiguration { get; set; }
        public DbSet<Configuration> Configuration { get; set; }
    }
}
