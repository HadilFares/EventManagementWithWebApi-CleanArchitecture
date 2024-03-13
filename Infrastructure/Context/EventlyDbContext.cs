using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Context
{
    public class EventlyDbContext : IdentityDbContext<User>
    {
        public EventlyDbContext(DbContextOptions<EventlyDbContext> options) : base(options)
        { }
        //public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Organizer> Organizers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventlyDbContext).Assembly);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Organizer>().ToTable("Organizers");
           //modelBuilder.Entity<Organizer>().HasKey(p => p.IdOrganizer);
            /* modelBuilder.Entity<Organizer>()
             .HasBaseType<User>();*/
            modelBuilder.Entity<User>()
      .HasOne(u => u.account)
      .WithOne(a => a.User)
      .HasForeignKey<Account>(a => a.UserId);

        }
    }
}
