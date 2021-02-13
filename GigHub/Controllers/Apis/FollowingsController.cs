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
            if (_context.Followings.Any(a => a.FolloweeId == userId && a.FolloweeId == dTO.FolloweeId))
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
    }
}
