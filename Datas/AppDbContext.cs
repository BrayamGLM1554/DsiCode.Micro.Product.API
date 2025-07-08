using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace DsiCode.Micro.Product.API.Datas
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<DsiCode.Micro.Product.API.Model.Product> Productos { get; set; }


    }
}
