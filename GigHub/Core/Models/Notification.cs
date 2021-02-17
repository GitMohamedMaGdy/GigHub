using System;
using System.ComponentModel.DataAnnotations;

namespace GigHub.Core.Models
{
    public class Notification
    {
        public int Id { get; private set; }
        public DateTime DateTime { get; private set; }
        public NotificationType Type { get; private set; }
        public DateTime? OriginalDateTime { get; private set; }
        public string OriginalVenue { get; private set; }


        [Required]
        public Gig Gig { get; private set; }

        protected Notification()
        {

        }

        private Notification(NotificationType notificationType, Gig gig)
        {

            if (gig == null)
            {
                throw new ArgumentNullException("gig");
            }

            Type = notificationType;
            Gig = gig;
            DateTime = DateTime.Now;



        }

        public static Notification GigCreated(Gig gig)
        {
            return new Notification(NotificationType.GigCreated, gig);
        }

        public static Notification GigCanceld(Gig gig)
        {
            return new Notification(NotificationType.GigCanceled, gig);
        }

        public static Notification GigUpdated(Gig gig, DateTime OriginalDateTime, string Venue)
        {
            var notification = new Notification(NotificationType.GigCreated, gig);
            notification.OriginalDateTime = OriginalDateTime;
            notification.OriginalVenue = Venue;
            return notification;
        }
    }
}