using System.Linq;
using System.Web.Mvc;
using NerdDinner.Models;
using NHibernate;

namespace NerdDinner.Controllers {
    public class SearchController : Controller {
        private readonly ISession _session;

        private readonly IDinnerRepository _dinnerRepository;

        public SearchController(ISession session, IDinnerRepository repository)
        {
            _session = session;
            _dinnerRepository = repository;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult SearchByLocation(float latitude, float longitude) {

            using (_session.BeginTransaction())
            {                
                var dinners = _dinnerRepository.FindByLocation(latitude, longitude);

                var jsonDinners = from dinner in dinners.ToList()
                                  select new JsonDinner()
                                  {
                                      DinnerID = dinner.DinnerID,
                                      Latitude = dinner.Latitude,
                                      Longitude = dinner.Longitude,
                                      Title = dinner.Title,
                                      Description = dinner.Description,
                                      RSVPCount = dinner.Rsvps.Count(),
                                  };

                return Json(jsonDinners.ToList());                
            }
        }
    }
}
