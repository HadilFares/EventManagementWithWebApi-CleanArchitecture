using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Context
{
    public class EventlyDbContext : DbContext
    {
        public EventlyDbContext(DbContextOptions<EventlyDbContext> options) : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Organizer> Organizers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventlyDbContext).Assembly);
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);
            //modelBuilder.Entity<Participant>().ToTable("Participants");
            modelBuilder.Entity<Organizer>().ToTable("Organizers");

        }
    }
}
