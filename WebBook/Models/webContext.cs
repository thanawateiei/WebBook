using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebBook.Models
{
    public partial class webContext : DbContext
    {
        public webContext()
        {
        }

        public webContext(DbContextOptions<webContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Agency> Agencies { get; set; } = null!;
        public virtual DbSet<Book> Books { get; set; } = null!;
        public virtual DbSet<BookType> BookTypes { get; set; } = null!;
        public virtual DbSet<History> Histories { get; set; } = null!;
        public virtual DbSet<Location> Locations { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserType> UserTypes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Data Source=MSI\\SQLEXPRESS01;Initial Catalog=web;Persist Security Info=True;User ID=webdev;Password=56785678");
                optionsBuilder.UseSqlServer("Data Source=LAPTOP-8FFPN76C\\SQLEXPRESS;Initial Catalog=web;Persist Security Info=True;User ID=webdev;Password=1234");

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Thai_CI_AS");

            modelBuilder.Entity<Agency>(entity =>
            {
                entity.ToTable("Agency");

                entity.Property(e => e.AgencyId)
                    .ValueGeneratedNever()
                    .HasColumnName("agency_id");

                entity.Property(e => e.AgencyName)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("agency_name");
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Book");

                entity.Property(e => e.BookId)
                    .ValueGeneratedNever()
                    .HasColumnName("book_id");

                entity.Property(e => e.AuthorName)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("author_name");

                entity.Property(e => e.BookCover)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("book_cover");

                entity.Property(e => e.BookDetail)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("book_detail");

                entity.Property(e => e.BookName)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("book_name");

                entity.Property(e => e.BookType1).HasColumnName("book_type_1");

                entity.Property(e => e.BookType2).HasColumnName("book_type_2");

                entity.Property(e => e.BookType3)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("book_type_3");

                entity.Property(e => e.BookType4)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("book_type_4");

                entity.Property(e => e.BookType5)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("book_type_5");

                entity.Property(e => e.PublicationYear).HasColumnName("publication_year");

                entity.Property(e => e.Publisher)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("publisher");
            });

            modelBuilder.Entity<BookType>(entity =>
            {
                entity.ToTable("BookType");

                entity.Property(e => e.BookTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("bookType_id");

                entity.Property(e => e.BookTypeName)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("bookType_name");
            });

            modelBuilder.Entity<History>(entity =>
            {
                entity.ToTable("History");

                entity.Property(e => e.HistoryId)
                    .ValueGeneratedNever()
                    .HasColumnName("history_id");

                entity.Property(e => e.AuthorName)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("author_name");

                entity.Property(e => e.BookName)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("book_name");

                entity.Property(e => e.CallNumber)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("callNumber");

                entity.Property(e => e.PublicationYear).HasColumnName("publication_year");

                entity.Property(e => e.Publisher)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("publisher");

                entity.Property(e => e.ReceiveDate)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("receive_date");

                entity.Property(e => e.StatusId)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("status_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location");

                entity.Property(e => e.LocationId)
                    .ValueGeneratedNever()
                    .HasColumnName("location_id");

                entity.Property(e => e.LocationDetail)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("locatio_detail");

                entity.Property(e => e.LocationName)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("location_name");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId)
                    .ValueGeneratedNever()
                    .HasColumnName("role_id");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("role_name");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.StatusId)
                    .ValueGeneratedNever()
                    .HasColumnName("status_id");

                entity.Property(e => e.StatusName)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("status_name");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("user_id");

                entity.Property(e => e.AgencyId).HasColumnName("agency_id");

                entity.Property(e => e.Email)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Password).HasColumnName("password");

                entity.Property(e => e.Role)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("role");

                entity.Property(e => e.StudentId)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("student_id");

                entity.Property(e => e.Telephone).HasColumnName("telephone");

                entity.Property(e => e.UserType).HasColumnName("user_type");
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.ToTable("UserType");

                entity.Property(e => e.UserTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("userType_id");

                entity.Property(e => e.Limit).HasColumnName("limit");

                entity.Property(e => e.UserTypeName)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("userType_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
