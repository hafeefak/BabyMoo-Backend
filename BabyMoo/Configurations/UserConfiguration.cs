using BabyMoo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BabyMoo.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // One-to-one: User ↔ Cart
            builder.HasOne(u => u.Carts)
                   .WithOne(c => c.User)
                   .HasForeignKey<CartModel>(c => c.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // One-to-many: User ↔ Wishlists
            builder.HasMany(u => u.Wishlists)
                   .WithOne(w => w.User)
                   .HasForeignKey(w => w.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // One-to-many: User ↔ Addresses
            builder.HasMany(u => u.Addresses)
                   .WithOne(a => a.User)
                   .HasForeignKey(a => a.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
