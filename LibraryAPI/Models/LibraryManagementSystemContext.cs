using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Models;

public partial class LibraryManagementSystemContext : DbContext
{
    public LibraryManagementSystemContext()
    {
    }

    public LibraryManagementSystemContext(DbContextOptions<LibraryManagementSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Borrow> Borrows { get; set; }

    public virtual DbSet<Fine> Fines { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=KERIM\\SQLEXPRESS;Initial Catalog=library_management_system;Integrated Security=True; TrustServerCertificate = True;");


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.AuthorId).HasName("PK__author__86516BCF4899A81A");

            entity.ToTable("author");

            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.Nationality)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nationality");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__books__490D1AE1C6700560");

            entity.ToTable("book");

            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.AvailabilityStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("availability_status");
            entity.Property(e => e.GenreId).HasColumnName("genre_id");
            entity.Property(e => e.Isbn)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("isbn");
            entity.Property(e => e.PublishYear).HasColumnName("publish_year");
            entity.Property(e => e.PublisherId).HasColumnName("publisher_id");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("title");

            entity.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK_books_author");

            entity.HasOne(d => d.Genre).WithMany(p => p.Books)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("FK_books_genre");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Books)
                .HasForeignKey(d => d.PublisherId)
                .HasConstraintName("FK_books_publisher");
        });

        modelBuilder.Entity<Borrow>(entity =>
        {
            entity.HasKey(e => e.BorrowId).HasName("PK__borrow__262B57A0A00BE494");

            entity.ToTable("borrow");

            entity.Property(e => e.BorrowId).HasColumnName("borrow_id");
            entity.Property(e => e.ActualReturnDate)
                .HasColumnType("datetime")
                .HasColumnName("actual_return_date");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.BorrowDate)
                .HasColumnType("datetime")
                .HasColumnName("borrow_date");
            entity.Property(e => e.DueReturnDate)
                .HasColumnType("datetime")
                .HasColumnName("due_return_date");
            entity.Property(e => e.MemberId).HasColumnName("member_id");

            entity.HasOne(d => d.Book).WithMany(p => p.Borrows)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK_borrow_books")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Member).WithMany(p => p.Borrows)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK_borrow_member")
                .OnDelete(DeleteBehavior.Cascade);

        });

        modelBuilder.Entity<Fine>(entity =>
        {
            entity.HasKey(e => e.FineId).HasName("PK__fine__F3C688D109268F49");

            entity.ToTable("fine");

            entity.Property(e => e.FineId).HasColumnName("fine_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.DatePaid)
                .HasColumnType("datetime")
                .HasColumnName("date_paid");
            entity.Property(e => e.MemberId).HasColumnName("member_id");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("payment_status");

            entity.HasOne(d => d.Member).WithMany(p => p.Fines)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK_fine_member");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("PK__genre__18428D4218DB6F72");

            entity.ToTable("genre");

            entity.Property(e => e.GenreId).HasColumnName("genre_id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.GenreName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("genre_name");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK__member__B29B8534FB8C71AD");

            entity.ToTable("member");

            entity.Property(e => e.MemberId).HasColumnName("member_id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.MembershipStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("membership_status");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone_number");

 
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.PublisherId).HasName("PK__publishe__3263F29D0530DA37");

            entity.ToTable("publisher");

            entity.Property(e => e.PublisherId).HasColumnName("publisher_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.ReservationId).HasName("PK__reservat__31384C29BBF48FE4");

            entity.ToTable("reservation");

            entity.Property(e => e.ReservationId).HasColumnName("reservation_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.MemberId).HasColumnName("member_id");
            entity.Property(e => e.PickupDate)
                .HasColumnType("datetime")
                .HasColumnName("pickup_date");
            entity.Property(e => e.ReservationDate)
                .HasColumnType("datetime")
                .HasColumnName("reservation_date");

            entity.HasOne(d => d.Book).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK_reservation_books");

            entity.HasOne(d => d.Member).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK_reservation_member");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__staff__1963DD9C3A56BDE3");

            entity.ToTable("staff");

            entity.Property(e => e.StaffId).HasColumnName("staff_id");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone_number");
            entity.Property(e => e.Position)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("position");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
