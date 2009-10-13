using System;
using System.Linq;

namespace NerdDinner.Models {

    public interface IDinnerRepository {

        IQueryable<Dinner> FindAllDinners();
        IQueryable<Dinner> FindByLocation(float latitude, float longitude);
        IQueryable<Dinner> FindUpcomingDinners();
        Dinner GetDinner(int id);

        void Save(Dinner dinner);
        void Delete(Dinner dinner);
    }
}
