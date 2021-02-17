using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System.Linq;

namespace GigHub.Peristance
{
    public class FollowingRepository : IFollowingRepository
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