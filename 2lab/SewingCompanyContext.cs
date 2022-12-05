using System;
using System.Collections.Generic;
using _2lab.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace _2lab
{
    public partial class SewingCompanyContext : DbContext
    {
        public SewingCompanyContext()
        {
        }

        public SewingCompanyContext(DbContextOptions<SewingCompanyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Material> Materials { get; set; } = null!;
        public virtual DbSet<MaterialList> MaterialLists { get; set; } = null!;
        public virtual DbSet<MaterialsForProduct> MaterialsForProducts { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductsOfWorker> ProductsOfWorkers { get; set; } = null!;
        public virtual DbSet<Provider> Providers { get; set; } = null!;
        public virtual DbSet<Worker> Workers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=GOSHKO_LAPTOP\\SQLEXPRESS;Initial Catalog=SewingCompany;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(30);

                entity.Property(e => e.Type).HasMaxLength(30);

                entity.HasOne(d => d.Provider)
                    .WithMany(p => p.Materials)
                    .HasForeignKey(d => d.ProviderId)
                    .HasConstraintName("FK__Materials__Provi__3C69FB99");
            });

            modelBuilder.Entity<MaterialList>(entity =>
            {
                entity.HasOne(d => d.Material)
                    .WithMany(p => p.MaterialLists)
                    .HasForeignKey(d => d.MaterialId)
                    .HasConstraintName("FK__MaterialL__Mater__412EB0B6");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.MaterialLists)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__MaterialL__Produ__4222D4EF");
            });

            modelBuilder.Entity<MaterialsForProduct>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("MaterialsForProduct");

                entity.Property(e => e.MatName).HasMaxLength(30);

                entity.Property(e => e.ProductName).HasMaxLength(30);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.DeliveryOrderDate).HasColumnType("date");

                entity.Property(e => e.ExecutionStartDate).HasColumnType("date");

                entity.Property(e => e.ImplementationDate).HasColumnType("date");

                entity.Property(e => e.OrderDate).HasColumnType("date");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__Orders__Customer__44FF419A");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__Orders__ProductI__45F365D3");

                entity.HasOne(d => d.Worker)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.WorkerId)
                    .HasConstraintName("FK__Orders__WorkerId__46E78A0C");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(30);

                entity.Property(e => e.Price).HasColumnType("money");
            });

            modelBuilder.Entity<ProductsOfWorker>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ProductsOfWorkers");

                entity.Property(e => e.ProductName).HasMaxLength(30);

                entity.Property(e => e.WorkerName).HasMaxLength(50);
            });

            modelBuilder.Entity<Provider>(entity =>
            {
                entity.Property(e => e.DeliveryDate).HasColumnType("date");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Price).HasColumnType("money");
            });

            modelBuilder.Entity<Worker>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Position).HasMaxLength(30);

                entity.Property(e => e.Section).HasMaxLength(30);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
