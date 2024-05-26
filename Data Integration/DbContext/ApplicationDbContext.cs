﻿using Data_Integration.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Integration
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SubscribeToOffer>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.CouponNumber).IsRequired();
                entity.Property(x => x.MSISDN).IsRequired();
            });

            modelBuilder.Entity<RewardLoyalty>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.WalletCode).IsRequired();
                entity.Property(x => x.PointsValue).IsRequired();
            });
        }

        public DbSet<SubscribeToOffer> SubscribeToOffers { get; set; }
        public DbSet<RewardLoyalty> RewardLoyaltys { get; set; }
    }
}