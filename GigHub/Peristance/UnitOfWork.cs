using GigHub.Core;
using GigHub.Core.Models;
using GigHub.Core.Repositories;

namespace GigHub.Peristance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IAttendanceRepository Attendances { get; private set; }
        public IGigsRepository Gigs { get; private set; }
        public IFollowingRepository Followings { get; private set; }
        public IGenreRepository Genres { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            this._context = context;
            Gigs = new GigsRepository(_context);
            Attendances = new AttendanceRepository(_context);
            Followings = new FollowingRepository(_context);
            Genres = new GenreRepository(_context);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }

    }
}