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
        public virtual DbSet<PopularBook> PopularBooks { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserType> UserTypes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
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
                    .HasMaxLength(512)
                    .IsUnicode(false)
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

                entity.Property(e => e.BookLang)
                    .HasMaxLength(10)
                    .HasColumnName("book_lang")
                    .IsFixedLength();

                entity.Property(e => e.BookName)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("book_name");

                entity.Property(e => e.BookType1).HasColumnName("bookType1");

                entity.Property(e => e.BookType10).HasColumnName("bookType10");

                entity.Property(e => e.BookType2).HasColumnName("bookType2");

                entity.Property(e => e.BookType3).HasColumnName("bookType3");

                entity.Property(e => e.BookType4).HasColumnName("bookType4");

                entity.Property(e => e.BookType5).HasColumnName("bookType5");

                entity.Property(e => e.BookType6).HasColumnName("bookType6");

                entity.Property(e => e.BookType7).HasColumnName("bookType7");

                entity.Property(e => e.BookType8).HasColumnName("bookType8");

                entity.Property(e => e.BookType9).HasColumnName("bookType9");

                entity.Property(e => e.CallNumber)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("callNumber");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.PublicationYear).HasColumnName("publication_year");

                entity.Property(e => e.Publisher)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("publisher");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");
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
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("history_id");

                entity.Property(e => e.AuthorName)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("author_name");

                entity.Property(e => e.BookLang)
                    .HasMaxLength(10)
                    .HasColumnName("book_lang")
                    .IsFixedLength();

                entity.Property(e => e.BookName)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("book_name");

                entity.Property(e => e.CallNumber)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("callNumber");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.LocationId).HasColumnName("location_id");

                entity.Property(e => e.PublicationYear).HasColumnName("publication_year");

                entity.Property(e => e.Publisher)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("publisher");

                entity.Property(e => e.ReceiveDate)
                    .HasColumnType("datetime")
                    .HasColumnName("receive_date");

                entity.Property(e => e.ReturnDate)
                    .HasColumnType("datetime")
                    .HasColumnName("return_date");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UserId)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("user_id");
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
                    .HasColumnName("location_detail");

                entity.Property(e => e.LocationName)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("location_name");
            });

            modelBuilder.Entity<PopularBook>(entity =>
            {
                entity.HasKey(e => e.PopularId)
                    .HasName("PK__PopularB__49127DF80CBF4518");

                entity.ToTable("PopularBook");

                entity.Property(e => e.PopularId)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("Popular_id");

                entity.Property(e => e.BookId)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("book_id");

                entity.Property(e => e.PopularCount).HasColumnName("Popular_count");

                entity.Property(e => e.PopularDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Popular_Date");
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
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("user_id");

                entity.Property(e => e.AgencyId).HasColumnName("agency_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Email)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Role).HasColumnName("role");

                entity.Property(e => e.StudentId)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("student_id");

                entity.Property(e => e.Telephone)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("telephone");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UserType).HasColumnName("user_type");
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.ToTable("UserType");

                entity.Property(e => e.UserTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("userType_id");

                entity.Property(e => e.Enbook).HasColumnName("ENbook");

                entity.Property(e => e.Limit).HasColumnName("limit");

                entity.Property(e => e.Thbook).HasColumnName("THbook");

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
