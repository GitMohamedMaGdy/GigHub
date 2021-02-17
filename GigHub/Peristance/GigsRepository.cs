using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GigHub.Peristance
{
    public class GigsRepository : IGigsRepository
    {
        private readonly ApplicationDbContext context;

        public GigsRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<Gig> GetGigsUserAttending(string userId)
        {
            var gigs = context.Attandances.Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .ToList();
            return gigs;

        }

        public Gig GetGigWithAttendances(int gigId)
        {

            return context.Gigs
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .Single(g => g.Id == gigId);
        }

        public Gig GetGigById(int id)
        {
            return context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .SingleOrDefault(g => g.Id == id);
        }

        public IEnumerable<Gig> GetUpComingGigsByArtist(string userId)
        {
            return context.Gigs.
                   Where(a => a.ArtistId == userId && a.DateTime > DateTime.Now && !a.isCancelled)
                  .Include(a => a.Genre)
                  .ToList();

        }

        public void Add(Gig gig)
        {
            context.Gigs.Add(gig);
        }
    }
}