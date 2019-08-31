using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OurTrace.Data.Identity.Models;
using OurTrace.Data.Models;
using System.Collections.Generic;

namespace OurTrace.Data
{
    public class OurTraceDbContext : IdentityDbContext<OurTraceUser, OurTraceRole, string>
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
        public DbSet<Follow> Follows { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Advert> Adverts { get; set; }


        public OurTraceDbContext(DbContextOptions<OurTraceDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var splitStringConverter = new ValueConverter<IEnumerable<string>, string>(v => string.Join(";", v), v => v.Split(new[] { ';' }));


            builder.Entity<OurTraceUser>((user) =>
            {
                user
                .HasMany(x => x.Groups)
                .WithOne(x => x.Creator)
                .HasForeignKey(x => x.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

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

                user
                .HasMany(x => x.Followers)
                .WithOne(x => x.Recipient)
                .HasForeignKey(x => x.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);

                user
                .HasMany(x => x.Following)
                .WithOne(x => x.Sender)
                .HasForeignKey(x => x.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

                user
                .HasMany(x => x.Notifications)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Post>((post) =>
            {
                post
                .HasMany(x => x.Comments)
                .WithOne(x => x.Post)
                .HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.Restrict);

                post
                .HasMany(x => x.Likes)
                .WithOne(x => x.Post)
                .HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.Restrict);

                post
                .HasMany(x => x.Shares)
                .WithOne(x => x.Post)
                .HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.Restrict);

                post
                .Property(nameof(Post.Tags))
                .HasConversion(splitStringConverter);
            });

            builder.Entity<Comment>((comment) =>
            {
                comment
                .HasMany(x => x.Likes)
                .WithOne(x => x.Comment)
                .HasForeignKey(x => x.CommentId)
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

                group
                .HasMany(x => x.Members)
                .WithOne(x => x.Group)
                .HasForeignKey(x => x.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            });
            builder.Entity<Wall>()
                .HasMany(x => x.Posts)
                .WithOne(x => x.Location)
                .HasForeignKey(x => x.LocationId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<UserGroup>((ug) =>
            {
                ug
                .HasKey(x => new { x.GroupId, x.UserId });
            });
        }

    }
}
