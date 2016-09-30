﻿using System.Data.Entity;
using Domain.Entities;

namespace Domain.EF
{
    public class UserContext: DbContext
    {
        public UserContext()
        {
            Database.Log = s => System.Diagnostics.Trace.WriteLine(s);
            Database.SetInitializer(new   DropCreateDatabaseIfModelChanges<UserContext>());
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<PriceHistory> PriceHistories { get; set; }
        public DbSet<UserProduct> UserProducts { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<PriceHistory>().
        //      HasMany(c => c.Products).
        //      WithMany(p => p.Users).
        //      Map(
        //       m =>
        //       {
        //           m.MapLeftKey("UserId");
        //           m.MapRightKey("ProductId");
        //       });
        //}
    }
}
