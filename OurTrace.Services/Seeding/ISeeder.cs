using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OurTrace.Services.Seeding
{
    public interface ISeeder
    {
        Task SeedAdmin();
        Task SeedUsers();
        Task SeedFriendships(int count);
        Task SeedAdminFriendships(int skip, int take);
        Task SeedProfilePictures(int skip, int take);
        Task SeedCoverPictures(int skip, int take);
        Task SeedFollowers(int count);
        Task SeedPosts(int count);
        Task SeedPostLikes(int maxLikes);
        Task SeedPostShares(int maxShares);
        Task SeedPostComments(int maxComments);
        Task SeedPostCommentsLikes(int maxLikes);
        Task SeedGroups(int count);
        Task SeedAdverts(int count);
    }
}
