using Microsoft.EntityFrameworkCore;

namespace MiApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets para cada entidad
        public DbSet<TipoUsuario> TiposUsuario { get; set; }
        public DbSet<TipoReporte> TiposReporte { get; set; }
        public DbSet<CategoriaAviso> CategoriasAviso { get; set; }
        public DbSet<TipoAmenidad> TiposAmenidad { get; set; }
        public DbSet<TipoServicio> TiposServicio { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<PersonalMantenimiento> PersonalMantenimiento { get; set; }
        public DbSet<AlertaPanico> AlertasPanico { get; set; }
        public DbSet<Reporte> Reportes { get; set; }
        public DbSet<Aviso> Avisos { get; set; }
        public DbSet<QRPersonal> QRPersonales { get; set; }
        public DbSet<Invitado> Invitados { get; set; }
        public DbSet<Amenidad> Amenidades { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<ServiciosCatalogo> ServiciosCatalogo { get; set; }
        public DbSet<SolicitudesServicio> SolicitudesServicio { get; set; }
        public DbSet<CuentaUsuario> CuentaUsuario { get; set; }
        public DbSet<CargoMantenimiento> CargosMantenimiento { get; set; }
        public DbSet<CargoServicio> CargosServicios { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<DetallePago> DetallePago { get; set; }
        public DbSet<ComprobantePago> ComprobantesPago { get; set; }
        public DbSet<MarcadorMapa> MarcadoresMapa { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureDecimalPrecision(modelBuilder);

            modelBuilder.Entity<CargoMantenimiento>()
                .Property(c => c.SaldoPendiente)
                .HasComputedColumnSql("[Monto] - [MontoPagado]", stored: true);

            modelBuilder.Entity<CargoServicio>()
                .Property(c => c.SaldoPendiente)
                .HasComputedColumnSql("[Monto] - [MontoPagado]", stored: true);

            modelBuilder.Entity<CargoServicio>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.CargosServicios)
                .HasForeignKey(c => c.UsuarioID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CargoServicio>()
                .HasOne(c => c.Solicitud)
                .WithMany(s => s.CargosServicios)
                .HasForeignKey(c => c.SolicitudID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CargoMantenimiento>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.CargosMantenimiento)
                .HasForeignKey(c => c.UsuarioID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CargoMantenimiento>()
                .HasOne(c => c.PersonalMantenimiento)
                .WithMany(p => p.CargosMantenimiento)
                .HasForeignKey(c => c.PersonalMantenimientoID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reporte>()
                .HasOne(r => r.Usuario)
                .WithMany(u => u.Reportes)
                .HasForeignKey(r => r.UsuarioID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MarcadorMapa>()
                .HasOne(m => m.Usuario)
                .WithMany(u => u.MarcadoresMapa)
                .HasForeignKey(m => m.UsuarioID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pago>()
                .HasOne(p => p.CargoMantenimiento)
                .WithMany(c => c.Pagos)
                .HasForeignKey(p => p.CargoMantenimientoID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pago>()
                .HasOne(p => p.CargoServicio)
                .WithMany(c => c.Pagos)
                .HasForeignKey(p => p.CargoServicioID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Comprobante)
                .WithOne(c => c.Pago)
                .HasForeignKey<ComprobantePago>(c => c.PagoID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TipoUsuario>()
                .HasIndex(t => t.Nombre)
                .IsUnique();

            modelBuilder.Entity<TipoReporte>()
                .HasIndex(t => t.Nombre)
                .IsUnique();

            modelBuilder.Entity<CategoriaAviso>()
                .HasIndex(c => c.Nombre)
                .IsUnique();

            modelBuilder.Entity<TipoServicio>()
                .HasIndex(t => t.Nombre)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.FirebaseUID)
                .IsUnique();

            modelBuilder.Entity<QRPersonal>()
                .HasIndex(q => q.CodigoQR)
                .IsUnique();

            modelBuilder.Entity<Invitado>()
                .HasIndex(i => i.CodigoQR)
                .IsUnique();

            modelBuilder.Entity<Pago>()
                .HasIndex(p => p.FolioUnico)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureDecimalPrecision(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CargoMantenimiento>()
                .Property(c => c.Monto)
                .HasPrecision(10, 2);

            modelBuilder.Entity<CargoMantenimiento>()
                .Property(c => c.MontoPagado)
                .HasPrecision(10, 2);

            modelBuilder.Entity<CargoMantenimiento>()
                .Property(c => c.SaldoPendiente)
                .HasPrecision(10, 2);

            modelBuilder.Entity<CargoServicio>()
                .Property(c => c.Monto)
                .HasPrecision(10, 2);

            modelBuilder.Entity<CargoServicio>()
                .Property(c => c.MontoPagado)
                .HasPrecision(10, 2);

            modelBuilder.Entity<CargoServicio>()
                .Property(c => c.SaldoPendiente)
                .HasPrecision(10, 2);

            modelBuilder.Entity<CuentaUsuario>()
                .Property(c => c.SaldoMantenimiento)
                .HasPrecision(10, 2);

            modelBuilder.Entity<CuentaUsuario>()
                .Property(c => c.SaldoServicios)
                .HasPrecision(10, 2);

            modelBuilder.Entity<CuentaUsuario>()
                .Property(c => c.SaldoTotal)
                .HasPrecision(10, 2);

            modelBuilder.Entity<PersonalMantenimiento>()
                .Property(p => p.Sueldo)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Pago>()
                .Property(p => p.MontoTotal)
                .HasPrecision(10, 2);

            modelBuilder.Entity<DetallePago>()
                .Property(d => d.MontoAplicado)
                .HasPrecision(10, 2);
        }
    }
}
