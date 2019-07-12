using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OurTrace.Data.Identity.Models;
using OurTrace.Data.Models;

namespace OurTrace.Data
{
    public class OurTraceDbContext : IdentityDbContext<OurTraceUser, IdentityRole, string>
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupAdmin> GroupAdmins { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Share> Shares { get; set; }
        public DbSet<Wall> Walls { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }


        public OurTraceDbContext(DbContextOptions<OurTraceDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<OurTraceUser>((user) =>
            {
                user
                .HasOne(x => x.Wall)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);

                user
                .HasMany(x => x.Posts)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

                user
                .HasMany(x => x.Comments)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

                user
                .HasMany(x => x.SentFriendships)
                .WithOne(x => x.Sender)
                .HasForeignKey(x => x.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

                user
                .HasMany(x => x.ReceivedFriendships)
                .WithOne(x => x.Recipient)
                .HasForeignKey(x => x.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Post>((post) =>
            {
                post
                .HasMany(x => x.Comments)
                .WithOne(x => x.Post)
                .HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Group>((group) =>
            {
                group
                .HasMany(x => x.Admins)
                .WithOne(x => x.Group)
                .HasForeignKey(x => x.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

                group
                .HasOne(x => x.Wall)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);

            });
            builder.Entity<Wall>()
                .HasMany(x => x.Posts)
                .WithOne(x => x.Location)
                .HasForeignKey(x => x.LocationId)
                .OnDelete(DeleteBehavior.Restrict);
            

            builder.Entity<UserGroup>()
                .HasKey(ug => new { ug.GroupId, ug.UserId });
        }

    }
}
