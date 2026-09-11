using Microsoft.EntityFrameworkCore;
using Model;

namespace DAL.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) 
            : base(options)
        {

        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<ClienteProducto> ClientesProductos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ClienteProducto>().HasOne<Cliente>().WithMany().HasForeignKey(c => c.ClienteId);
            modelBuilder.Entity<ClienteProducto>().HasOne<Producto>().WithMany().HasForeignKey(p => p.ProductoId);
            modelBuilder.Entity<ClienteProducto>().Property(cp => cp.Balance).HasPrecision(10, 2);
            modelBuilder.Entity<Producto>().Property(p => p.Price).HasPrecision(10, 2);
        }


    }
}
