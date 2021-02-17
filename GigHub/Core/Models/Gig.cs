using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GigHub.Core.Models
{
    public class Gig
    {

        public int Id { get; set; }

        public bool isCancelled { get; private set; }


        public ApplicationUser Artist { get; set; }

        [Required]
        public string ArtistId { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        [StringLength(255)]
        public string Venue { get; set; }
        public Genre Genre { get; set; }

        [Required]
        public int GenreId { get; set; }

        public ICollection<Attendance> Attendances { get; private set; }

        public Gig()
        {
            Attendances = new Collection<Attendance>();
        }

        public void Cancel()
        {
            isCancelled = true;

            var notification = Notification.GigCanceld(this);


            foreach (var attendee in Attendances.Select(g => g.Attendee))
            {
                attendee.Notify(notification);

            }
        }

        public void Modify(DateTime dateTime, string venue, int genre)
        {
            //set old values in notification
            var notification = Notification.GigUpdated(this, dateTime, venue);



            //new Values
            Venue = venue;
            DateTime = dateTime;
            GenreId = genre;


            foreach (var attendee in Attendances.Select(g => g.Attendee))
            {
                attendee.Notify(notification);

            }
            throw new NotImplementedException();
        }
    }
}