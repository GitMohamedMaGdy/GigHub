using GigHub.Core.Models;
using GigHub.Core.ViewModels;
using GigHub.Peristance;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext context;
        private readonly AttendanceRepository _attendanceRepository;

        public HomeController()
        {
            context = new ApplicationDbContext();
            _attendanceRepository = new AttendanceRepository(context);
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
            var attendances = _attendanceRepository.GetFutureAttendances(userId).ToLookup(a => a.GigId);


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