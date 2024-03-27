using Microsoft.EntityFrameworkCore;

namespace Cafe
{
    internal class DBContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<ShiftModel> Shifts { get; set; }

        public DBContext()
        {
            //Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;user=root;password=rootpwd;database=CafeDB;",
                new MySqlServerVersion(new Version(8, 3, 0)));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .HasOne(u => u.ActiveShift)
                .WithMany(s => s.Workers)
                .HasForeignKey(u => u.ActiveShiftId);
        }
    }
}
