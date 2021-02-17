using GigHub.Core.Repositories;

namespace GigHub.Core
{
    public interface IUnitOfWork
    {
        IAttendanceRepository Attendances { get; }
        IGigsRepository Gigs { get; }
        IFollowingRepository Followings { get; }
        IGenreRepository Genres { get; }

        void Complete();

    }
}