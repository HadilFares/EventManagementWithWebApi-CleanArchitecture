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
        public EventlyDbContext(DbContextOptions<EventlyDbContext> options)
            : base(options)
        {
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Event> Events { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventlyDbContext).Assembly);
        //User
            modelBuilder.Entity<User>()
            .HasOne(u => u.Account)
            .WithOne(a => a.User)
            .HasForeignKey<Account>(a => a.UserId)
            .IsRequired();

            /*  modelBuilder.Entity<Account>()
           .HasOne(u => u.User)
           .WithOne(a => a.Account)
           .HasForeignKey<User>(a => a.AccountId);
             */
            //Category-Organizer
            modelBuilder.Entity<User>()
           .HasMany(a => a.Categories) 
           .WithOne(b => b.User) 
           .HasForeignKey(b => b.OrganizerId)
           .IsRequired();
            //Event-Category
           modelBuilder.Entity<Category>()
          .HasMany(a => a.Events)
          .WithOne(b => b.Category)
          .HasForeignKey(b => b.CategoryId)
          .IsRequired();

            //Event-Organizer
            modelBuilder.Entity<User>()
           .HasMany(a => a.Events)
           .WithOne(b => b.User)
           .HasForeignKey(b => b.OrganizerId)
           .IsRequired();

        }

    }

    }

