using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        // GET: Gigs
        ApplicationDbContext context = new ApplicationDbContext();


        public ActionResult Index()
        {

            return View();


        }

        [Authorize]
        public ActionResult Create()
        {
            var gigVM = new GigViewModel()
            {
                Genres = context.Genre.ToList()
            };
            return View(gigVM);



        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigViewModel gigVM)
        {
            if (!ModelState.IsValid)

            {
                var gigViewModel = context.Genre.ToList();
                return View("Create", gigViewModel);
            }

            var gig = new Gig()
            {
                ArtistId = User.Identity.GetUserId(),
                Venue = gigVM.Venue,
                GenreId = gigVM.Genre,
                DateTime = gigVM.GetDateTime()
            };
            context.Gig.Add(gig);
            context.SaveChanges();
            return RedirectToAction("Index", "Home");



        }
    }
}