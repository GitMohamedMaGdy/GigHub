using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.Apis
{
    [Authorize]
    public class FollowingsController : ApiController
    {
        ApplicationDbContext _context;
        public FollowingsController()
        {
            _context = new ApplicationDbContext();
        }


        [HttpPost]
        public IHttpActionResult Follow(FollowingDTO dTO)
        {
            var userId = User.Identity.GetUserId();
            if (_context.Followings.Any(a => a.FollowerId == userId && a.FolloweeId == dTO.FolloweeId))
            {
                return BadRequest("The following already exists");
            }
            var following = new Following
            {
                FollowerId = userId,
                FolloweeId = dTO.FolloweeId
            };
            _context.Followings.Add(following);
            _context.SaveChanges();
            return Ok();
        }


        [HttpDelete]
        public IHttpActionResult UnFollow(string id)
        {
            var userId = User.Identity.GetUserId();
            var following = _context.Followings.SingleOrDefault(a => a.FollowerId == userId && a.FolloweeId == id);

            if (following == null)
                return NotFound();

            _context.Followings.Remove(following);
            _context.SaveChanges();
            return Ok(id);
        }
    }
}
