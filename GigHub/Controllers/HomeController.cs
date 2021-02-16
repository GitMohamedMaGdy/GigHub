using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {

        ApplicationDbContext context;

        public HomeController()
        {
            context = new ApplicationDbContext();
        }
        public ActionResult Index(string query = null)
        {
            var upCommingGigs = context.Gigs
                .Include(i => i.Artist)
                .Include(i => i.Genre)
                .Where(i => i.DateTime > DateTime.Now && !i.isCancelled);

            if (!string.IsNullOrWhiteSpace(query))
            {
                upCommingGigs = upCommingGigs
                    .Where(i =>
                    i.Artist.Name.Contains(query) ||
                    i.Genre.Name.Contains(query) ||
                    i.Venue.Contains(query));
            }

            var userId = User.Identity.GetUserId();
            var attendances = context.Attandances
                .Where(a => a.AttendeeId == userId && a.Gig.DateTime > DateTime.Now)
                .ToList()
                .ToLookup(a => a.GigId);

            var ViewModel = new GigsViewModel()
            {
                upCommingGigs = upCommingGigs,
                showAction = User.Identity.IsAuthenticated,
                Heading = "UpComing Gigs",
                SearchTerm = query,
                Attendances = attendances
            };

            return View("Gigs", ViewModel);


        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }


}