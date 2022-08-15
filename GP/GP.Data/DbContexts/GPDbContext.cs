using GP.Data.Entities;
using Microsoft.EntityFrameworkCore;
using RealWord.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.Data
{
    public class GPDbContext : DbContext
    {
        public GPDbContext(DbContextOptions<GPDbContext> options)
          : base(options)
        {
        }

        public DbSet<Business> Businesses { get; set; }
        public DbSet<BusinessFollowers> BusinessFollowers { get; set; }
        public DbSet<BusinessOwner> BusinessOwners { get; set; }
        public DbSet<BusinessTags> BusinessTags { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<OpenDay> OpenDays { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewCool> ReviewCool { get; set; }
        public DbSet<ReviewFunny> ReviewFunny { get; set; }
        public DbSet<ReviewUseful> ReviewUseful { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Business>(b =>
            {
                b.HasKey(b => b.BusinessId);

                b.HasOne<BusinessOwner>(b => b.BusinessOwner)
                .WithOne(bo => bo.Business)
                .HasForeignKey<BusinessOwner>(bo => bo.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

                b.HasMany<Review>(b => b.Reviews)
                .WithOne(r => r.Business)
                .HasForeignKey(r => r.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

              /*  b.HasMany<Feature>(b => b.Features)
                .WithOne(f => f.Business)
                .HasForeignKey(f => f.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);*/

                b.HasMany<Photo>(b => b.Photos)
               .WithOne(p => p.Business)
               .HasForeignKey(p => p.BusinessId)
               .OnDelete(DeleteBehavior.Cascade);

               /* b.HasMany<Service>(b => b.Services)
               .WithOne(s => s.Business)
               .HasForeignKey(s => s.BusinessId)
               .OnDelete(DeleteBehavior.Cascade);*/

                b.HasMany<OpenDay>(b => b.OpenDays)
               .WithOne(od => od.Business)
               .HasForeignKey(od => od.BusinessId)
               .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<BusinessOwner>(bo =>
            {
                bo.HasKey(bo => bo.BusinessOwnerId);

                bo.Property(bo => bo.Email).IsRequired();
                bo.Property(bo => bo.Password).IsRequired();
            });

            modelBuilder.Entity<BusinessFollowers>(bf =>
            {
                bf.HasKey(bf => new { bf.BusinessId, bf.UserId });

                bf.HasOne(bf => bf.Business)
                .WithMany(u => u.Followers)
                .HasForeignKey(bf => bf.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

                bf.HasOne(bf => bf.User)
                .WithMany(u => u.Followings)
                .HasForeignKey(bf => bf.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });

           /* modelBuilder.Entity<BusinessTags>(bt =>
            {
                bt.HasKey(at => new { at.BusinessId, at.TagId });

                bt.HasOne(bf => bf.Business)
                .WithMany(b => b.Tags)
                .HasForeignKey(bf => bf.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

                bt.HasOne(bt => bt.Tag)
                .WithMany(t => t.BusinessTags)
                .HasForeignKey(bt => bt.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            });*/

           /* modelBuilder.Entity<Feature>(f =>
            {
                f.HasKey(f => f.FeatureId);

                f.Property(f => f.FeatureName).IsRequired();
            });*/

            modelBuilder.Entity<OpenDay>(od =>
            {
                od.HasKey(od => od.OpenDayId);

                od.Property(od => od.StartTime).IsRequired();
                od.Property(od => od.EndTime).IsRequired();
            });

            modelBuilder.Entity<Photo>(p =>
            {
                p.HasKey(p => p.PhotoId);

                p.Property(p => p.PhotoPath).IsRequired();
            });

            modelBuilder.Entity<Service>(s =>
            {
                s.HasKey(s => s.ServiceId);

                s.Property(s => s.ServiceName).IsRequired();
            });

           /* modelBuilder.Entity<Tag>(t =>
            {
                t.HasKey(t => t.TagId);
            });*/

            modelBuilder.Entity<Review>(r =>
            {
                r.HasKey(r => r.ReviewId);

                r.Property(r => r.ReviewText).IsRequired();
                r.Property(r => r.CreatedAt).IsRequired();

                r.HasMany<Photo>(r => r.Photos)
                .WithOne(p =>p.Review)
                .HasForeignKey(p => p.ReviewId)
                .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<ReviewCool>(rc =>
            {
                rc.HasKey(rc => new { rc.ReviewId, rc.UserId });

                rc.HasOne(rc => rc.User)
                .WithMany(u => u.Cool)
                .HasForeignKey(rc => rc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

                rc.HasOne(rc => rc.Review)
                .WithMany(r => r.Cool)
                .HasForeignKey(rc => rc.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ReviewFunny>(rf =>
            {
                rf.HasKey(rf => new { rf.ReviewId, rf.UserId });

                rf.HasOne(rf => rf.User)
                .WithMany(u => u.Funny)
                .HasForeignKey(rf => rf.UserId)
                .OnDelete(DeleteBehavior.Cascade);

                rf.HasOne(rf => rf.Review)
                .WithMany(r => r.Funny)
                .HasForeignKey(rf => rf.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ReviewUseful>(ru =>
            {
                ru.HasKey(ru => new { ru.ReviewId, ru.UserId });

                ru.HasOne(ru => ru.User)
                .WithMany(u => u.Useful)
                .HasForeignKey(ru => ru.UserId)
                .OnDelete(DeleteBehavior.Cascade);

                ru.HasOne(ru => ru.Review)
                .WithMany(r => r.Useful)
                .HasForeignKey(ru => ru.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<User>(u =>
            {
                u.HasKey(u => u.UserId);

                u.HasIndex(u => u.Email).IsUnique();
                u.HasIndex(u => u.Username).IsUnique();

                u.Property(a => a.Email).IsRequired();
                u.Property(a => a.Password).IsRequired();
                u.Property(a => a.Username).IsRequired();

                u.HasMany<Review>(u => u.Reviews)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}