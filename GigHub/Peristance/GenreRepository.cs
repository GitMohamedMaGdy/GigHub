using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace GigHub.Peristance
{
    public class GenreRepository : IGenreRepository
    {

        private readonly ApplicationDbContext context;

        public GenreRepository(ApplicationDbContext context)
        {
            this.context = context;
        }


        public IEnumerable<Genre> GetGenres()
        {
            return context.Genres.ToList();
        }
    }
}