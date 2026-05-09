using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Rol> Roles { get; set; }
    public DbSet<CategoriaProducto> Categorias { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<Pago> Pagos { get; set; }
    public DbSet<Venta> Ventas { get; set; }
    public DbSet<DetalleVenta> DetallesVenta { get; set; }
    public DbSet<MovimientoInventario> MovimientosInventario { get; set; }
    public DbSet<ExportacionFactura> Facturas { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Deshabilitar OUTPUT clause para DetalleVenta (incompatible con triggers)
        modelBuilder.Entity<DetalleVenta>()
            .ToTable(tb => tb.UseSqlOutputClause(false));

        modelBuilder.Entity<ExportacionFactura>()
    .ToTable("Exportacion_Factura", tb => tb.UseSqlOutputClause(false));

        modelBuilder.Entity<ExportacionFactura>()
    .HasOne(f => f.Venta)
    .WithMany()
    .HasForeignKey(f => f.IdVenta);

        modelBuilder.Entity<DetalleVenta>(entity =>
        {
            entity.ToTable("DetalleVenta", tb => tb.UseSqlOutputClause(false));

            entity.HasKey(d => d.IdDetalle);

            entity.HasOne<Venta>()
                  .WithMany(v => v.Detalles)
                  .HasForeignKey(d => d.IdVenta);

            entity.HasOne(d => d.Producto)
                  .WithMany()
                  .HasForeignKey(d => d.IdProducto);
        });

        modelBuilder.Entity<Venta>()
            .HasOne(v => v.Usuario)
            .WithMany()
            .HasForeignKey(v => v.IdUsuario);

        modelBuilder.Entity<Venta>()
            .HasOne(v => v.Pago)
            .WithMany()
            .HasForeignKey(v => v.IdPago);

        modelBuilder.Entity<Producto>()
            .HasOne(p => p.Categoria)
            .WithMany()
            .HasForeignKey(p => p.IdCategoria);

        modelBuilder.Entity<MovimientoInventario>()
            .HasOne(m => m.Producto)
            .WithMany()
            .HasForeignKey(m => m.IdProducto);

        modelBuilder.Entity<Usuario>()
            .HasOne<Rol>()
            .WithMany()
            .HasForeignKey(u => u.IdRol);
    }
}