using System;
using System.Collections.Generic;
using System.Linq;
using NerdDinner.Models;

namespace NerdDinner.Infrastructure
{
    public class NhDinnerRepository : IDinnerRepository
    {
        private readonly IQueryable<Dinner> _dinners = new List<Dinner>().AsQueryable();

        public IQueryable<Dinner> FindAllDinners()
        {
            return _dinners;
        }

        public IQueryable<Dinner> FindByLocation(float latitude, float longitude)
        {
            return _dinners.Where(d => d.Distance(latitude, longitude) < 100).AsQueryable();
        }

        public IQueryable<Dinner> FindUpcomingDinners()
        {
            return from dinner in _dinners
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
            //_dinners.Add(dinner);
        }
        public void Delete(Dinner dinner)
        {
            //_dinners.Remove(dinner);
        }

        public void Save()
        {
            foreach (Dinner dinner in _dinners)
            {
                if (!dinner.IsValid)
                    throw new ApplicationException("Rule violations");
            }
        }
    }
}
