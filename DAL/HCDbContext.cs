using BOL;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace DAL
{
    public class HCDbContext : IdentityDbContext
    {
        public HCDbContext(DbContextOptions<HCDbContext> options) : base(options)
        {
            Database.Migrate();
        }



        //USED TO MAKE THE MIGRATION FILE AND THEN COMMENTED AND ON PRODUCTION .Migrate() METHOD IS USED
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer("Server=INSPIRON580S;Database=HaalCHaalDb;Trusted_Connection=True;TrustServerCertificate=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Follower>()
                .HasKey(f => new { f.FollowerUserId, f.FollowingUserId });  //composite primary key

            modelBuilder.Entity<Follower>()
         .HasOne(f => f.FollowerUser)
         .WithMany(u => u.Followers)
         .HasForeignKey(f => f.FollowerUserId)
         .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Follower>()
                .HasOne(f => f.FollowingUser)
                .WithMany(u => u.Following)
                .HasForeignKey(f => f.FollowingUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        public DbSet<HCUser>? HCUsers { get; set; }
        public DbSet<StoryPost>? StoryPosts { get; set; }
        public DbSet<Comment>? Comments { get; set; }

        public DbSet<Follower> Followers { get; set; }
    }
}