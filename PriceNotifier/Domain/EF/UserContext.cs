using System.Data.Entity;
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().
              HasMany(c => c.Products).
              WithMany(p => p.Users).
              Map(
               m =>
               {
                   m.MapLeftKey("UserId");
                   m.MapRightKey("ProductId");
               });
        }
    }
}
