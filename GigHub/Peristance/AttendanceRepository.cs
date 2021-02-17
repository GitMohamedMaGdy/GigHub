using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GigHub.Peristance
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly ApplicationDbContext context;

        public AttendanceRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<Attendance> GetFutureAttendances(string userId)
        {
            var attendances = context.Attandances
               .Where(a => a.AttendeeId == userId && a.Gig.DateTime > DateTime.Now)
               .ToList();
            return attendances;
        }

        public Attendance GetAttendance(int gigId, string userId)
        {
            return context.Attandances
                    .SingleOrDefault(a => a.GigId == gigId && a.AttendeeId == userId);
        }


    }
}