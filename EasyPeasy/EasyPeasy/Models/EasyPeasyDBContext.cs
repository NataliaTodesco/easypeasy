using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EasyPeasy.Models
{
    public partial class EasyPeasyDBContext : DbContext
    {
        public EasyPeasyDBContext()
        {
        }

        public EasyPeasyDBContext(DbContextOptions<EasyPeasyDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Barrio> Barrios { get; set; }
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<DetalleEntrega> DetalleEntregas { get; set; }
        public virtual DbSet<Estado> Estados { get; set; }
        public virtual DbSet<HojaRuta> HojaRuta { get; set; }
        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<ProductosXremito> ProductosXremitos { get; set; }
        public virtual DbSet<Remito> Remitos { get; set; }
        public virtual DbSet<Transportista> Transportistas { get; set; }
        public virtual DbSet<Vehiculo> Vehiculos { get; set; }
        public virtual DbSet<Zona> Zonas { get; set; }
        public virtual DbSet<Disponibilidad> Disponibilidades {get;set;}
        //public virtual DbSet<Motivos> Motivos { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseNpgsql(Startup.ConnString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Spanish_Spain.1252");
            //  modelBuilder.Entity<Motivos>(entity=>{
            //     entity.HasKey(m=>m.IdMotivo)
            //     .HasName("IdMotivo");
            //     entity.Property(e => e.IdMotivo).UseIdentityAlwaysColumn();
            //       entity.Property(e => e.Descripcion)
            //         .IsRequired()
            //         .HasColumnType("character varying");
           

            // });

            modelBuilder.Entity<Disponibilidad>(entity=>{
                entity.HasKey(k=>k.Id)
                .HasName("Id");
                 entity.Property(e => e.Id).UseIdentityAlwaysColumn();
                  entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("character varying");
            });


            modelBuilder.Entity<Barrio>(entity =>
            {
                entity.HasKey(e => e.IdBarrio)
                    .HasName("IdBarrio");

                entity.Property(e => e.IdBarrio).UseIdentityAlwaysColumn();

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.HasOne(d => d.IdZonaNavigation)
                    .WithMany(p => p.Barrios)
                    .HasForeignKey(d => d.IdZona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IdZona");
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.IdCliente)
                    .HasName("IdCliente");

                entity.Property(e => e.IdCliente).UseIdentityAlwaysColumn();

                entity.Property(e => e.Direccion)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.Telefono).HasColumnType("character varying");

                entity.HasOne(d => d.IdBarrioNavigation)
                    .WithMany(p => p.Clientes)
                    .HasForeignKey(d => d.IdBarrio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IdBarrio");
            });

            modelBuilder.Entity<DetalleEntrega>(entity =>
            {
                entity.HasKey(e => e.IdDetalle)
                    .HasName("IdDetalle");

                entity.ToTable("DetalleEntrega");

                entity.Property(e => e.IdDetalle).UseIdentityAlwaysColumn();

                entity.Property(e => e.HoraEntrega).HasColumnType("time without time zone");

                entity.Property(e => e.Observaciones).HasColumnType("character varying");

                entity.HasOne(d => d.IdRemitoNavigation)
                    .WithMany(p => p.DetalleEntregas)
                    .HasForeignKey(d => d.IdRemito)
                    .HasConstraintName("IdRemito");

                    //            entity.HasOne(d => d.IdMotivoNavigation)
                    // .WithMany(p => p.DetalleEntregas)
                    // .HasForeignKey(d => d.IdMotivo)
                    // .HasConstraintName("IdMotivo");

                 
            });

            modelBuilder.Entity<Estado>(entity =>
            {
                entity.HasKey(e => e.IdEstado)
                    .HasName("IdEstado");

                entity.Property(e => e.IdEstado).UseIdentityAlwaysColumn();

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<HojaRuta>(entity =>
            {
                entity.HasKey(e => e.IdHojaRuta)
                    .HasName("IdHojaRuta");

                entity.Property(e => e.IdHojaRuta).UseIdentityAlwaysColumn();

                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.HasOne(d => d.IdTransportistaNavigation)
                    .WithMany(p => p.HojaRuta)
                    .HasForeignKey(d => d.IdTransportista)
                    .HasConstraintName("IdTransportista");

                entity.HasOne(d => d.IdVehiculoNavigation)
                    .WithMany(p => p.HojaRuta)
                    .HasForeignKey(d => d.IdVehiculo)
                    .HasConstraintName("IdVehiculo");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.IdProducto)
                    .HasName("IdProducto");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<ProductosXremito>(entity =>
            {
                entity.HasKey(e => new { e.IdProducto, e.IdRemito })
                    .HasName("ProductosXRemitos_pkey");

                entity.ToTable("ProductosXRemitos");

                entity.HasIndex(e => e.IdProducto, "fki_IdProducto");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.ProductosXremitos)
                    .HasForeignKey(d => d.IdProducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IdProducto");

                entity.HasOne(d => d.IdRemitoNavigation)
                    .WithMany(p => p.ProductosXremitos)
                    .HasForeignKey(d => d.IdRemito)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IdRemito");
            });

            modelBuilder.Entity<Remito>(entity =>
            {
                entity.HasKey(e => e.IdRemito)
                    .HasName("IdRemito");

                entity.Property(e => e.IdRemito).UseIdentityAlwaysColumn();

                entity.Property(e => e.FechaCompra).HasColumnType("date");

                entity.Property(e => e.HoraEntregaPreferido).HasColumnType("time without time zone");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Remitos)
                    .HasForeignKey(d => d.IdCliente)
                    .HasConstraintName("IdCliente");

                entity.HasOne(d => d.IdEstadoNavigation)
                    .WithMany(p => p.Remitos)
                    .HasForeignKey(d => d.IdEstado)
                    .HasConstraintName("IdEstado");

                entity.HasOne(d => d.IdHojaRutaNavigation)
                    .WithMany(p => p.Remitos)
                    .HasForeignKey(d => d.IdHojaRuta)
                    .HasConstraintName("IdHojaRuta");
            });

            modelBuilder.Entity<Transportista>(entity =>
            {
                entity.HasKey(e => e.IdTransportista)
                    .HasName("IdTransportista");

                entity.Property(e => e.IdTransportista).UseIdentityAlwaysColumn();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("character varying");

                                             entity.HasOne(d => d.IdDisponibilidadNavigation)
                    .WithMany(p => p.Transportistas)
                    .HasForeignKey(d => d.idDisponibilidad)
                    .HasConstraintName("idDisponibilidad");
            });

            modelBuilder.Entity<Vehiculo>(entity =>
            {
                entity.HasKey(e => e.IdVehiculo)
                    .HasName("IdVehiculo");

                entity.Property(e => e.IdVehiculo).UseIdentityAlwaysColumn();

                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.Modelo)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.Patente)
                    .IsRequired()
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<Zona>(entity =>
            {
                entity.HasKey(e => e.IdZona)
                    .HasName("Zona_pkey");

                entity.Property(e => e.IdZona).UseIdentityAlwaysColumn();

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnType("character varying");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
