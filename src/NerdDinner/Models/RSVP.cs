using System;

namespace NerdDinner.Models
{
    public class RSVP
    {
        public virtual int RsvpId { get; set; }
        public virtual string AttendeeName { get; set; }

        public virtual Dinner Dinner { get; set; }
    }
}
