using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace _6lab;

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
        => optionsBuilder.UseSqlServer("Data Source=goshko\\sqlexpress;Initial Catalog=SewingCompany;Integrated Security=True;Pooling=False; Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC074109DBF1");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Material__3214EC0748A024F0");

            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Type).HasMaxLength(30);

            entity.HasOne(d => d.Provider).WithMany(p => p.Materials)
                .HasForeignKey(d => d.ProviderId)
                .HasConstraintName("FK__Materials__Provi__2A4B4B5E");
        });

        modelBuilder.Entity<MaterialList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Material__3214EC0717D507F7");

            entity.HasOne(d => d.Material).WithMany(p => p.MaterialLists)
                .HasForeignKey(d => d.MaterialId)
                .HasConstraintName("FK__MaterialL__Mater__2F10007B");

            entity.HasOne(d => d.Product).WithMany(p => p.MaterialLists)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__MaterialL__Produ__300424B4");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC074A9F0422");

            entity.Property(e => e.DeliveryOrderDate).HasColumnType("date");
            entity.Property(e => e.ExecutionStartDate).HasColumnType("date");
            entity.Property(e => e.ImplementationDate).HasColumnType("date");
            entity.Property(e => e.OrderDate).HasColumnType("date");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Orders__Customer__32E0915F");

            entity.HasOne(d => d.Product).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Orders__ProductI__33D4B598");

            entity.HasOne(d => d.Worker).WithMany(p => p.Orders)
                .HasForeignKey(d => d.WorkerId)
                .HasConstraintName("FK__Orders__WorkerId__34C8D9D1");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC07D33407F2");

            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Price).HasColumnType("money");
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Provider__3214EC07AAB87E4A");

            entity.Property(e => e.DeliveryDate).HasColumnType("date");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("money");
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Workers__3214EC0776BDC462");

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Position).HasMaxLength(30);
            entity.Property(e => e.Section).HasMaxLength(30);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
