﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BabyMoo.Models;

namespace BabyMoo.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.TotalAmount)
                   .HasPrecision(18, 2)  
                   .IsRequired();

            builder.HasOne(o => o.User)
                   .WithMany(u => u.Order)
                   .HasForeignKey(o => o.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

         
            builder.HasOne(o => o.Address)
                   .WithMany()
                   .HasForeignKey(o => o.AddressId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(o => o.PaymentStatus).HasDefaultValue("Pending");
            builder.Property(o => o.Status).HasDefaultValue("Pending");
        }
    }
}
