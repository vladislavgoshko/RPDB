using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;

namespace lab4.DbModels;

public partial class SewingCompanyContext : DbContext
{
    public SewingCompanyContext()
    {
    }

    public SewingCompanyContext(DbContextOptions<SewingCompanyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<MaterialList> MaterialLists { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=GOSHKO\\SQLEXPRESS;Initial Catalog=SewingCompany;Integrated Security=True; Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC072764D24F");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Material__3214EC07DF969698");

            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Type).HasMaxLength(30);

            entity.HasOne(d => d.Provider).WithMany(p => p.Materials)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Materials__Provi__3C69FB99");
        });

        modelBuilder.Entity<MaterialList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Material__3214EC07C1879CBB");

            entity.HasOne(d => d.Material).WithMany(p => p.MaterialLists)
                .HasForeignKey(d => d.MaterialId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__MaterialL__Mater__412EB0B6");

            entity.HasOne(d => d.Product).WithMany(p => p.MaterialLists)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__MaterialL__Produ__4222D4EF");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC07989E1C3E");

            entity.Property(e => e.DeliveryOrderDate).HasColumnType("date");
            entity.Property(e => e.ExecutionStartDate).HasColumnType("date");
            entity.Property(e => e.ImplementationDate).HasColumnType("date");
            entity.Property(e => e.OrderDate).HasColumnType("date");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Orders__Customer__44FF419A");

            entity.HasOne(d => d.Product).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Orders__ProductI__45F365D3");

            entity.HasOne(d => d.Worker).WithMany(p => p.Orders)
                .HasForeignKey(d => d.WorkerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Orders__WorkerId__46E78A0C");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC078CB3E64D");

            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Price).HasColumnType("money");
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Provider__3214EC07B1343C24");

            entity.Property(e => e.DeliveryDate).HasColumnType("date");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("money");
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Workers__3214EC07F84F063E");

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Position).HasMaxLength(30);
            entity.Property(e => e.Section).HasMaxLength(30);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
