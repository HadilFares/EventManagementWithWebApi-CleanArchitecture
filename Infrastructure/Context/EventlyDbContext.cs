using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
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

     
            //Category-Organizer
            modelBuilder.Entity<User>()
           .HasMany(a => a.Categories) 
           .WithOne(b => b.User) 
           .HasForeignKey(b => b.OrganizerId)
          .OnDelete(DeleteBehavior.ClientSetNull);
            //Event-Category
            modelBuilder.Entity<Category>()
           .HasMany(a => a.Events)
           .WithOne(b => b.Category)
           .HasForeignKey(b => b.CategoryId)
             .OnDelete(DeleteBehavior.ClientSetNull);



            //Event-Organizer
            modelBuilder.Entity<User>()
              .HasMany(a => a.Events)
              .WithOne(b => b.User)
              .HasForeignKey(b => b.UserId)
              .OnDelete(DeleteBehavior.ClientSetNull);


            //Comment-participant

            modelBuilder.Entity<User>()
             .HasMany(a => a.Comments)
             .WithOne(b => b.User)
             .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull);


            //event-comment
            modelBuilder.Entity<Event>()
            .HasMany(a => a.Comments)
            .WithOne(b => b.Event)
            .HasForeignKey(b => b.EventId)
              .OnDelete(DeleteBehavior.ClientSetNull);


            /* modelBuilder.Entity<User>().ToTable(d => d.HasTrigger("[User_Delete]"));

             modelBuilder.Entity<Category>().ToTable(e => e.HasTrigger("[Category_Delete]"));

             modelBuilder.Entity<Event>().ToTable(p => p.HasTrigger("[Event_Delete]"));

             modelBuilder.Entity<Comment>().ToTable(t => t.HasTrigger("[Comment_Delete]"));*/

          //User-Subscription
            modelBuilder.Entity<User>()
               .HasOne(u => u.Subscription) 
               .WithOne(s => s.User)       
               .HasForeignKey<Subscription>(s => s.UserId); 

            //SubscriptionPlan-Subscription
            modelBuilder.Entity<SubscriptionPlan>()
        .HasMany(a => a.Subscriptions)
        .WithOne(b => b.SubscriptionPlan)
        .HasForeignKey(b => b.PlanId);
        //.OnDelete(DeleteBehavior.ClientSetNull);

        }

    }

    }

