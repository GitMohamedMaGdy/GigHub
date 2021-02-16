using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
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
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var gigs = context.Attandances.Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .ToList();

            var attendances = context.Attandances
                .Where(a => a.AttendeeId == userId && a.Gig.DateTime > DateTime.Now)
                .ToList()
                .ToLookup(a => a.GigId);

            var viewModel = new GigsViewModel()
            {
                showAction = User.Identity.IsAuthenticated,
                upCommingGigs = gigs,
                Heading = "Gigs I'm Attending",
                Attendances = attendances
            };

            return View("Gigs", viewModel);
        }

        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var gigs = context.Gigs.
                Where(a => a.ArtistId == userId && a.DateTime > DateTime.Now && !a.isCancelled)
                .Include(a => a.Genre)
                .ToList();

            return View(gigs);
        }

        public ActionResult Details(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .SingleOrDefault(g => g.Id == id);

            if (gig == null)
            {
                return HttpNotFound();
            }
            var GigDetailsVM = new GigDetailsViewModel { Gig = gig };

            if (User.Identity.IsAuthenticated)
            {
                GigDetailsVM.IsAttending = context.Attandances
                    .Any(a => a.GigId == gig.Id && a.AttendeeId == userId);

                GigDetailsVM.IsFollowing = context.Followings
                    .Any(a => a.FolloweeId == gig.Artist.Id && a.FollowerId == userId);
            }

            return View("Details", GigDetailsVM);
        }

        [HttpPost]
        public ActionResult Search(GigsViewModel gigsViewModel)
        {
            return RedirectToAction("Index", "Home", new { query = gigsViewModel.SearchTerm });
        }
        //[Authorize]
        //public ActionResult Following()
        //{
        //    var userId = User.Identity.GetUserId();
        //    var gigs = context.Followings.Where(a => a.FollowerId == userId).ToList();




        //    return View("Gigs", viewModel);
        //}
        [Authorize]
        public ActionResult Create()
        {

            var gigVM = new GigViewModel()
            {
                Genres = context.Genres.ToList(),
                Heading = "Add Gig"

            };

            return View("GigForm", gigVM);



        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigViewModel gigVM)
        {
            if (!ModelState.IsValid)

            {
                gigVM.Genres = context.Genres.ToList();
                return View("GigForm", gigVM);
            }

            var gig = new Gig()
            {
                ArtistId = User.Identity.GetUserId(),
                Venue = gigVM.Venue,
                GenreId = gigVM.Genre,
                DateTime = gigVM.GetDateTime()
            };
            context.Gigs.Add(gig);
            context.SaveChanges();
            return RedirectToAction("Mine", "Gigs");



        }


        [Authorize]
        [HttpGet]

        public ActionResult Edit(int gigId)
        {
            var userId = User.Identity.GetUserId();
            var gig = context.Gigs.Single(g => g.Id == gigId && g.ArtistId == userId);
            var gigViewModel = new GigViewModel()
            {
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                Venue = gig.Venue,
                Id = gig.Id,
                Genre = gig.GenreId,
                Genres = context.Genres.ToList(),
                Heading = "Edit Gig"
            };

            return View("GigForm", gigViewModel);



        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigViewModel gigVM)
        {
            if (!ModelState.IsValid)

            {
                gigVM.Genres = context.Genres.ToList();
                return View("GigForm", gigVM);
            }
            var userId = User.Identity.GetUserId();
            var gig = context.Gigs
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .Single(g => g.Id == gigVM.Id && g.ArtistId == userId);

            gig.Modify(gigVM.GetDateTime(), gigVM.Venue, gigVM.Genre);

            context.SaveChanges();
            return RedirectToAction("Mine", "Gigs");



        }

    }
}