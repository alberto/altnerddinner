using System;
using System.Collections.Generic;
using System.Linq;
using NerdDinner.Models;

namespace NerdDinner.Infrastructure
{
    public class InMemoryDinnerRepository : IDinnerRepository
    {
        private readonly IList<Dinner> _dinners = new List<Dinner>();

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

        public void Save(Dinner dinner)
        {
            if (!dinner.IsValid)
                throw new ApplicationException("Rule violations");

            var index = _dinners.IndexOf(_dinners.Where(d => d.DinnerID == dinner.DinnerID).FirstOrDefault());
            if (index >= 0)
            {
                _dinners.RemoveAt(index);
                _dinners.Insert(index, dinner);
                return;
            }

            dinner.DinnerID = _dinners.Count;
            _dinners.Add(dinner);

        }

        public void AddRange(IEnumerable<Dinner> dinners)
        {
            foreach (var dinner in dinners)
            {
                Save(dinner);
            }
        }
        public void Delete(Dinner dinner)
        {
            _dinners.Remove(dinner);
        }
    }
}