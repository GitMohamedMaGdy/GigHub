using GigHub.Models;
using GigHub.ViewModels;
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
        public ActionResult Index()
        {
            var upCommingGigs = context.Gigs
                .Include(i => i.Artist)
                .Include(i => i.Genre)
                .Where(i => i.DateTime > DateTime.Now && !i.isCancelled).ToList();

            var ViewModel = new GigsViewModel()
            {
                upCommingGigs = upCommingGigs,
                showAction = User.Identity.IsAuthenticated,
                Heading = "UpComing Gigs"
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