using System.Linq;
using System.Web.Mvc;
using NerdDinner.Models;

namespace NerdDinner.Controllers {
    public class SearchController : Controller {

        IDinnerRepository dinnerRepository;

        public SearchController(IDinnerRepository repository) {
            dinnerRepository = repository;
        }

        //
        // AJAX: /Search/FindByLocation?longitude=45&latitude=-90

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchByLocation(float latitude, float longitude) {

            var dinners = dinnerRepository.FindByLocation(latitude, longitude);

            var jsonDinners = from dinner in dinners
                              select new JsonDinner {
                                  DinnerID = dinner.DinnerID,
                                  Latitude = dinner.Latitude,
                                  Longitude = dinner.Longitude,
                                  Title = dinner.Title,
                                  Description = dinner.Description,
                                  RSVPCount = dinner.NumberOfAtendees
                              };

            return Json(jsonDinners.ToList());
        }
    }
}
