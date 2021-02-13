using GigHub.Models;
using System.Collections.Generic;

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
    }
}