using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NerdDinner.Models
{
    public class InMemoryDinnerRepository : IDinnerRepository
    {
        List<Dinner> _dinners = new List<Dinner>();
        public IQueryable<Dinner> FindAllDinners()
        {
            return _dinners.AsQueryable();
        }

        public IQueryable<Dinner> FindByLocation(float latitude, float longitude)
        {
            return _dinners.Where(d => d.Distance(latitude, longitude) < 100).AsQueryable();
        }

        public IQueryable<Dinner> FindUpcomingDinners()
        {
            return from dinner in FindAllDinners()
                   where dinner.EventDate > DateTime.Now
                   orderby dinner.EventDate
                   select dinner;
        }

        public Dinner GetDinner(int id)
        {
            return _dinners.SingleOrDefault(d => d.DinnerID == id);
        }

        public void Add(Dinner dinner)
        {
            _dinners.Add(dinner);
        }

        public void Delete(Dinner dinner)
        {
            _dinners.Remove(dinner);
        }

        public void Save()
        {
        }
    }
}
