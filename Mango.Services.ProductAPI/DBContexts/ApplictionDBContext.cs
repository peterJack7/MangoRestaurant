using Microsoft.EntityFrameworkCore;
using Mango.Services.ProductAPI.Models;
namespace Mango.Services.ProductAPI.DBContexts
{
    public class ApplictionDBContext : DbContext
    {
        public ApplictionDBContext(DbContextOptions<ApplictionDBContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
    }

}
