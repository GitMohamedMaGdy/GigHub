using GigHub.Models;
using GigHub.Repositories;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        // GET: Gigs
        private readonly ApplicationDbContext context;
        private readonly AttendanceRepository _attendanceRepository;
        private readonly GigsRepository _gigsRepository;
        private readonly FollowingRepository _followingRepository;
        private readonly GenreRepository _genreRepository;

        public GigsController()
        {
            context = new ApplicationDbContext();
            _attendanceRepository = new AttendanceRepository(context);
            _gigsRepository = new GigsRepository(context);
            _followingRepository = new FollowingRepository(context);
            _genreRepository = new GenreRepository(context);
        }


        public ActionResult Index()
        {

            return View();


        }
        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var viewModel = new GigsViewModel()
            {
                showAction = User.Identity.IsAuthenticated,
                upCommingGigs = _gigsRepository.GetGigsUserAttending(userId),
                Heading = "Gigs I'm Attending",
                Attendances = _attendanceRepository.GetFutureAttendances(userId).ToLookup(a => a.GigId)
            };

            return View("Gigs", viewModel);
        }



        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var gigs = _gigsRepository.GetUpComingGigsByArtist(userId);
            return View(gigs);
        }

        public ActionResult Details(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = _gigsRepository.GetGigById(id);

            if (gig == null)
            {
                return HttpNotFound();
            }
            var GigDetailsVM = new GigDetailsViewModel { Gig = gig };

            if (User.Identity.IsAuthenticated)
            {
                GigDetailsVM.IsAttending = _attendanceRepository.GetAttendance(gig.Id, userId) != null;

                GigDetailsVM.IsFollowing = _followingRepository.GetFollowing(userId, gig.ArtistId) != null;
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
                Genres = _genreRepository.GetGenres(),
                Heading = "Add a Gig"

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
                gigVM.Genres = _genreRepository.GetGenres();
                gigVM.Heading = "Add a Gig";
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
            var gig = _gigsRepository.GetGigById(gigId);
            if (gig == null)
            {
                return HttpNotFound();
            }

            if (gig.ArtistId != User.Identity.GetUserId())
            {
                return new HttpUnauthorizedResult();
            }
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

            var gig = _gigsRepository.GetGigWithAttendances(gigVM.Id);

            if (gig == null)
            {
                return HttpNotFound();
            }
            if (gig.ArtistId != User.Identity.GetUserId())
            {
                return new HttpUnauthorizedResult();
            }
            gig.Modify(gigVM.GetDateTime(), gigVM.Venue, gigVM.Genre);

            context.SaveChanges();
            return RedirectToAction("Mine", "Gigs");



        }

    }
}