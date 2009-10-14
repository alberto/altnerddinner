using System.Web.Mvc;
using NerdDinner.Models;
using NHibernate;

namespace NerdDinner.Controllers
{
    public class RsvpController : Controller {
        private readonly ISession _session;

        private readonly IDinnerRepository _dinnerRepository;

        public RsvpController(ISession session, IDinnerRepository repository)
        {
            _session = session;
            _dinnerRepository = repository;
        }

        //
        // AJAX: /Dinners/Register/1

        [Authorize, AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Register(int id) {

            using (var tx = _session.BeginTransaction())
            {
                Dinner dinner = _dinnerRepository.GetDinner(id);
                if (!dinner.IsUserRegistered(User.Identity.Name)) {
                    dinner.AddRsvp(new RSVP{ AttendeeName = User.Identity.Name });
                    _dinnerRepository.Save(dinner);
                    tx.Commit();
                }

                return Content("Thanks - we'll see you there!");
            }
        }
    }
}
