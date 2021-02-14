using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.Apis
{

    [Authorize]
    public class GigsController : ApiController
    {
        ApplicationDbContext _context;
        public GigsController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = _context.Gigs
                .Include(a => a.Attendances.Select(g => g.Attendee))
                .Single(a => a.Id == id && a.ArtistId == userId);

            if (gig.isCancelled)
                return NotFound();

            gig.Cancel();


            _context.SaveChanges();
            return Ok();


        }


    }
}
