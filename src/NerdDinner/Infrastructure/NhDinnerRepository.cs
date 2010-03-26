using System;
using System.Linq;
using NerdDinner.Models;
using NHibernate;
using NHibernate.Linq;

namespace NerdDinner.Infrastructure
{
    public class NhDinnerRepository : IDinnerRepository
    {
        private readonly ISession _session;

        public NhDinnerRepository(ISession session)
        {
            _session = session;
        }

        private INHibernateQueryable<Dinner> GetDbContext()
        {
            return _session.Linq<Dinner>();
        }

        public IQueryable<Dinner> FindAllDinners()
        {
            return GetDbContext().AsQueryable();
        }

        public IQueryable<Dinner> FindByLocation(float latitude, float longitude)
        {
            // TODO: Doesn't work in L2NH yet
            //return FindUpcomingDinners().Where(d => d.Distance(latitude, longitude) < 100).AsQueryable();
            var query = GetDbContext()
                .Expand("Rsvps")
                .Where(d => d.EventDate > DateTime.Now)                
                .ToList() //Needed because Distance() in not an Expression tree
                .Where(d => d.Distance(latitude, longitude) < 100)
                .AsQueryable();
            return query;
        }

        public IQueryable<Dinner> FindUpcomingDinners()
        {
            return from dinner in GetDbContext()
                   where dinner.EventDate > DateTime.Now
                   orderby dinner.EventDate
                   select dinner;
        }
        
        public Dinner GetDinner(int id)
        {
            return GetDbContext().SingleOrDefault(d => d.DinnerID == id);
        }

        public void Save(Dinner dinner)
        {
            if (!dinner.IsValid)
            {
                throw new ApplicationException("Rule violations");
            }

            _session.SaveOrUpdate(dinner);
        }

        public void Delete(Dinner dinner)
        {
            _session.Delete(dinner);
        }
    }
}