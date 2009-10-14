using FluentNHibernate.Mapping;
using NerdDinner.Models;

namespace NerdDinner.Mappers
{
    public class DinnerMap : ClassMap<Dinner>
    {
        public DinnerMap()
        {
            Id(x => x.DinnerID);
            Map(x => x.Address).Not.Nullable();
            Map(x => x.ContactPhone).Not.Nullable();
            Map(x => x.Country).Not.Nullable();
            Map(x => x.Description).Not.Nullable();
            Map(x => x.EventDate).Not.Nullable();
            Map(x => x.HostedBy).Not.Nullable();
            Map(x => x.Latitude).Not.Nullable();
            Map(x => x.Longitude).Not.Nullable();
            Map(x => x.Title).Not.Nullable();
            HasMany(x => x.Rsvps).Access.CamelCaseField(Prefix.Underscore)
                .Inverse().Cascade.AllDeleteOrphan();
        }
    }
}
