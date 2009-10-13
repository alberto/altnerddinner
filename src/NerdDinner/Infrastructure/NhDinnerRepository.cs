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
            return CurrentSession.Linq<Dinner>();
        }

        private ISession CurrentSession
        {
            get { return _session; }
        }

        public IQueryable<Dinner> FindAllDinners()
        {
            return GetDbContext();
        }

        public IQueryable<Dinner> FindByLocation(float latitude, float longitude)
        {
            return GetDbContext().Where(d => d.Distance(latitude, longitude) < 100).AsQueryable();
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

            CurrentSession.SaveOrUpdate(dinner);
        }

        public void Delete(Dinner dinner)
        {
            CurrentSession.Delete(dinner);
            CurrentSession.Flush();
        }
    }
}
