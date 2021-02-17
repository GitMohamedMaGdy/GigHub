using AutoMapper;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.Apis
{
    public class NotificationsController : ApiController
    {
        ApplicationDbContext _context;
        IMapper mapper;
        public NotificationsController()
        {
            _context = new ApplicationDbContext();
            mapper = AutoMapperProfile.Configure().CreateMapper();


        }

        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            var userId = User.Identity.GetUserId();

            var notifications = _context.UserNotifications
                .Where(us => us.UserId == userId && !us.IsRead)
                .Select(un => un.Notification)
                .Include(n => n.Gig.Artist)
                .ToList();


            return notifications.Select(mapper.Map<Notification, NotificationDto>);

        }


        [HttpPost]
        public IHttpActionResult MarkAsRead()
        {
            var userId = User.Identity.GetUserId();

            var notifications = _context.UserNotifications
                .Where(us => us.UserId == userId && !us.IsRead)
                .ToList();

            notifications.ForEach(n => n.Read());
            _context.SaveChanges();
            return Ok();
        }

    }
}
