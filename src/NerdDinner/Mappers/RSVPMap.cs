using FluentNHibernate.Mapping;
using NerdDinner.Models;

namespace NerdDinner.Mappers
{
    public class RSVPMap : ClassMap<RSVP>
    {
        public RSVPMap()
        {
            Id(x => x.RsvpId);
            Map(x => x.AttendeeName).Not.Nullable();
            References(r => r.Dinner).Not.Nullable();  
        }
    }
}
