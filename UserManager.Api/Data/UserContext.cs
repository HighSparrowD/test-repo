using Microsoft.EntityFrameworkCore;
using UserManager.Main.Entities.Users;

namespace UserManager.Data.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureUsers(modelBuilder);
        }

        //Hilo configuration
        private void ConfigureUsers(ModelBuilder builder)
        {
            const string sequenceName = "users_hilo";

            builder.HasSequence<int>(sequenceName)
                .StartsAt(1)
                .IncrementsBy(1);

            builder.Entity<User>(b =>
            {
                b.HasKey(e => e.Id);
                b.Property(e => e.FirstName).IsRequired().HasMaxLength(500);
                b.Property(e => e.LastName).IsRequired().HasMaxLength(700);
                b.Property(e => e.Email).IsRequired().HasMaxLength(500);
                b.Property(e => e.Password).IsRequired();

                b.Property(a => a.Id).UseHiLo(sequenceName);

                b.ToTable("user");
            });
        }
    }
}
