using Microsoft.EntityFrameworkCore;
using TrickyTrayAPI.Models;


namespace TrickyTrayAPI.Data
{
    public class TrickyTrayDbContext : DbContext
    {
        public TrickyTrayDbContext(DbContextOptions<TrickyTrayDbContext> options) : base(options) { }

        public DbSet<Buyer> Buyers => Set<Buyer>();

        public DbSet<Donor> Donors => Set<Donor>();

        public DbSet<Gift> Gifts => Set<Gift>();

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderGift> OrderGift => Set<OrderGift>();

        public DbSet<SystemState> SystemState => Set<SystemState>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Gift>()
                .HasOne(g => g.Donor)
                .WithMany(d => d.Gifts)
                .HasForeignKey(g => g.DonorId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Buyer)
                .WithMany()
                .HasForeignKey(o => o.BuyerId);

            modelBuilder.Entity<OrderGift>()
                .HasOne(og => og.Order)
                .WithMany(o => o.OrderGifts)
                .HasForeignKey(og => og.OrderId);

            modelBuilder.Entity<OrderGift>()
                .HasOne(og => og.Gift)
                .WithMany()
                .HasForeignKey(og => og.GiftId);

            modelBuilder.Entity<SystemState>()
            .HasKey(s => s.Id);

        }
    }
}
