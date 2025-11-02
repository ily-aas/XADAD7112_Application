using XADAD7112_Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using XADAD7112_Application.Models.Booking;
using XADAD7112_Application.Models.System;

namespace XADAD7112_Application.Services
{
    public class AppDbContext:DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options){}

        public DbSet<User> User { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<BookingItem> BookingItem { get; set; }
        public DbSet<TraceLogs> Logs { get; set; }
        public DbSet<Inquiry> Inquiries { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("User").HasKey(x => x.Id);
            modelBuilder.Entity<Booking>().ToTable("Booking").HasKey(x => x.Id);
            modelBuilder.Entity<BookingItem>().ToTable("BookingItems").HasKey(x => x.Id);
            modelBuilder.Entity<TraceLogs>().ToTable("TraceLogs").HasKey(x => x.Id);
            modelBuilder.Entity<Inquiry>().ToTable("Inquiry").HasKey(x => x.Id);

        }
    }
}
