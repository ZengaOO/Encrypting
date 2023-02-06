using Encrypting.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Encrypting.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
         : base(options)
        {
        }
        public DbSet<ContractModel> ContractModels { get; set; }


    }
}
