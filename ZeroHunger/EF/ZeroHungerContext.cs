using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ZeroHunger.EF;

public partial class ZeroHungerContext : DbContext
{
    public ZeroHungerContext()
    {
    }

    public ZeroHungerContext(DbContextOptions<ZeroHungerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Food> Foods { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Restaurant> Restaurants { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;database=Zero_Hunger;uid=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.28-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PRIMARY");

            entity.Property(e => e.AdminId).HasColumnType("int(11)");
            entity.Property(e => e.AdminName).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmpId).HasName("PRIMARY");

            entity.Property(e => e.EmpId).HasColumnType("int(11)");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.EmpName).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
        });

        modelBuilder.Entity<Food>(entity =>
        {
            entity.HasKey(e => e.FoodId).HasName("PRIMARY");

            entity.HasIndex(e => e.ReqId, "FK_Foods_Requests");

            entity.Property(e => e.FoodId).HasColumnType("int(11)");
            entity.Property(e => e.FoodName).HasMaxLength(50);
            entity.Property(e => e.ReqId).HasColumnType("int(11)");

            entity.HasOne(d => d.Req).WithMany(p => p.Foods)
                .HasForeignKey(d => d.ReqId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Foods_Requests");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.ReqId).HasName("PRIMARY");

            entity.HasIndex(e => e.EmpId, "FK_Requests_Employees");

            entity.HasIndex(e => e.ResId, "FK_Requests_Restaurants");

            entity.Property(e => e.ReqId).HasColumnType("int(11)");
            entity.Property(e => e.CollectTime).HasColumnType("datetime");
            entity.Property(e => e.CompleteTime).HasColumnType("datetime");
            entity.Property(e => e.EmpId).HasColumnType("int(11)");
            entity.Property(e => e.PreserveTime).HasColumnType("datetime");
            entity.Property(e => e.ReqTime).HasColumnType("datetime");
            entity.Property(e => e.ResId).HasColumnType("int(11)");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Emp).WithMany(p => p.Requests)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Requests_Employees");

            entity.HasOne(d => d.Res).WithMany(p => p.Requests)
                .HasForeignKey(d => d.ResId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Requests_Restaurants");
        });

        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.HasKey(e => e.ResId).HasName("PRIMARY");

            entity.Property(e => e.ResId).HasColumnType("int(11)");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.ResName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
