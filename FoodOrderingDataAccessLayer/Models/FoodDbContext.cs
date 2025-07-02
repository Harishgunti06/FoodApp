using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FoodOrderingDataAccessLayer.Models;

public partial class FoodDbContext : DbContext
{
    public FoodDbContext()
    {
    }

    public FoodDbContext(DbContextOptions<FoodDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<CartDetail> CartDetails { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Cuisine> Cuisines { get; set; }

    public virtual DbSet<Issue> Issues { get; set; }

    public virtual DbSet<MenuItem> MenuItems { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json");
        var config = builder.Build();
        var connectionString =
       config.GetConnectionString("FoodDBConnection");
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Bookings__73951AEDE9E18FF4");

            entity.Property(e => e.CheckedIn).HasDefaultValue(false);
            entity.Property(e => e.Email).HasMaxLength(100);

            entity.HasOne(d => d.EmailNavigation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.Email)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bookings__Email__4AB81AF0");
        });

        modelBuilder.Entity<CartDetail>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__CartDeta__51BCD7B781DB3ADE");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.ItemName).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.EmailNavigation).WithMany(p => p.CartDetails)
                .HasForeignKey(d => d.Email)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartDetai__Price__36B12243");

            entity.HasOne(d => d.MenuItem).WithMany(p => p.CartDetails)
                .HasForeignKey(d => d.MenuItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartDetai__MenuI__37A5467C");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0BCA742D85");

            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<Cuisine>(entity =>
        {
            entity.HasKey(e => e.CuisineId).HasName("PK__Cuisine__B1C3E7CB70147329");

            entity.ToTable("Cuisine");

            entity.HasIndex(e => e.CuisineName, "UQ__Cuisine__2C77DCC827DD0A2B").IsUnique();

            entity.Property(e => e.CuisineName).HasMaxLength(100);
        });

        modelBuilder.Entity<Issue>(entity =>
        {
            entity.HasKey(e => e.IssueId).HasName("PK__Issues__6C8616048441073F");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.IssueDescription).HasMaxLength(255);
            entity.Property(e => e.IssueStatus)
                .HasMaxLength(50)
                .HasDefaultValue("Open");

            entity.HasOne(d => d.EmailNavigation).WithMany(p => p.Issues)
                .HasForeignKey(d => d.Email)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Issues__Email__403A8C7D");

            entity.HasOne(d => d.OrderItem).WithMany(p => p.Issues)
                .HasForeignKey(d => d.OrderItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Issues__OrderIte__412EB0B6");
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(e => e.MenuItemId).HasName("PK__MenuItem__8943F722115C8A1E");

            entity.HasIndex(e => e.ItemName, "UQ__MenuItem__4E4373F7C952A72A").IsUnique();

            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.IsVegetarian).HasDefaultValue(true);
            entity.Property(e => e.ItemName).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.MenuItems)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MenuItems__Categ__30F848ED");

            entity.HasOne(d => d.Cuisine).WithMany(p => p.MenuItems)
                .HasForeignKey(d => d.CuisineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MenuItems__Cuisi__31EC6D26");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__OrderIte__57ED06815273F433");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.ItemName).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.EmailNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.Email)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__Email__3B75D760");

            entity.HasOne(d => d.MenuItem).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.MenuItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__MenuI__3C69FB99");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PK__Ratings__FCCDF87C14BA3AF2");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.ItemName).HasMaxLength(100);
            entity.Property(e => e.RatingValue).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.EmailNavigation).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.Email)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ratings__Email__45F365D3");

            entity.HasOne(d => d.MenuItem).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.MenuItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ratings__MenuIte__46E78A0C");

            entity.HasOne(d => d.OrderItem).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.OrderItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ratings__OrderIt__44FF419A");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1A4F307FBC");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B6160B7F7D720").IsUnique();

            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Email).HasName("pk_EmailId");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__RoleId__276EDEB3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
