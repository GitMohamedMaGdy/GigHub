using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers
{
    [Authorize]
    public class AttendancesController : ApiController
    {
        ApplicationDbContext _context;
        public AttendancesController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Attend(AttendanceDTO dTO)
        {
            var userId = User.Identity.GetUserId();
            if (_context.Attandances.Any(a => a.GigId == dTO.GigId && a.AttendeeId == userId))
            {
                return BadRequest("The attendance already exists");
            }
            var attendance = new Attendance
            {
                AttendeeId = userId,
                GigId = dTO.GigId
            };
            _context.Attandances.Add(attendance);
            _context.SaveChanges();
            return Ok();
        }
    }
}
