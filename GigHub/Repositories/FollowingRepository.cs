using GigHub.Models;
using System.Linq;

namespace GigHub.Repositories
{
    public class FollowingRepository
    {
        private readonly ApplicationDbContext context;

        public FollowingRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Following GetFollowing(string userId, string ArtistId)
        {
            return context.Followings
                    .SingleOrDefault(a => a.FolloweeId == ArtistId && a.FollowerId == userId);
        }
    }
}