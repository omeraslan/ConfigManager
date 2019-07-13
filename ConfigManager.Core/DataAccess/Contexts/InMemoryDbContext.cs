using System;
using System.Collections.Generic;
using System.Text;
using ConfigManager.Core.DataAccess.InMemoryEntities;
using Microsoft.EntityFrameworkCore;

namespace ConfigManager.Core.DataAccess.Contexts
{
   internal class InMemoryDbContext : DbContext
    {
        public InMemoryDbContext()
        {
        }

        public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options)
            : base(options)
        {
        }

        public DbSet<InMemoryConfigurationEntity> Configurations { get; set; }
    }
}
