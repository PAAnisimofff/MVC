using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GpuStore.Domain.Entities;

namespace GpuStore.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
    }
}
