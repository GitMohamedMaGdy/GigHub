using GigHub.Models;
using System.Collections.Generic;
using System.Linq;

namespace GigHub.ViewModels
{
    public class GigsViewModel
    {
        public GigsViewModel()
        {
        }

        public IEnumerable<Gig> upCommingGigs { get; set; }
        public bool showAction { get; set; }

        public string Heading { get; set; }

        public string SearchTerm { get; set; }
        public ILookup<int, Attendance> Attendances { get; internal set; }
    }
}