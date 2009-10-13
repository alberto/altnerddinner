using System;
using System.Web.Mvc;
using NerdDinner.Helpers;
using NerdDinner.Models;
using NHibernate;

namespace NerdDinner.Controllers {
    //
    // Controller Class

    [HandleError]
    public class DinnersController : Controller {
        private readonly ISession _session;

        readonly IDinnerRepository _dinnerRepository;

        public DinnersController(ISession session, IDinnerRepository repository)
        {
            _session = session;
            _dinnerRepository = repository;
        }

        //
        // GET: /Dinners/
        //      /Dinners/Page/2
        public ActionResult Index(int? page) {

            using (var tx = _session.BeginTransaction())
            {
                const int pageSize = 10;
                var upcomingDinners = _dinnerRepository.FindUpcomingDinners();
                var paginatedDinners = new PaginatedList<Dinner>(upcomingDinners, page ?? 0, pageSize);
                return View(paginatedDinners);
            }
        }

        //
        // GET: /Dinners/Details/5

        public ActionResult Details(int id) {
            using (var tx = _session.BeginTransaction())
            {
                Dinner dinner = _dinnerRepository.GetDinner(id);

                if (dinner == null)
                    return View("NotFound");

                return View(dinner);
            }
        }

        //
        // GET: /Dinners/Edit/5

        [Authorize]
        public ActionResult Edit(int id) {

            using (var tx = _session.BeginTransaction())
            {
                Dinner dinner = _dinnerRepository.GetDinner(id);

                if (!dinner.IsHostedBy(User.Identity.Name))
                    return View("InvalidOwner");

                return View(new DinnerFormViewModel(dinner));
            }
        }

        //
        // POST: /Dinners/Edit/5

        [AcceptVerbs(HttpVerbs.Post), Authorize]
        public ActionResult Edit(int id, FormCollection collection) {

            using (var tx = _session.BeginTransaction())
            {
                Dinner dinner = _dinnerRepository.GetDinner(id);

                if (!dinner.IsHostedBy(User.Identity.Name))
                    return View("InvalidOwner");

                try {
                    UpdateModel(dinner);

                    _dinnerRepository.Save(dinner);
                    tx.Commit();
                    return RedirectToAction("Details", new { id=dinner.DinnerID });
                }
                catch {
                    ModelState.AddModelErrors(dinner.GetRuleViolations());

                    return View(new DinnerFormViewModel(dinner));
                }
            }
        }

        //
        // GET: /Dinners/Create

        [Authorize]
        public ActionResult Create() {

            Dinner dinner = new Dinner() {
                EventDate = DateTime.Now.AddDays(7)
            };

            return View(new DinnerFormViewModel(dinner));
        } 

        //
        // POST: /Dinners/Create

        [AcceptVerbs(HttpVerbs.Post), Authorize]
        public ActionResult Create(Dinner dinner) {

            if (ModelState.IsValid) {
                try {
                    using (var tx = _session.BeginTransaction())
                    {
                        dinner.HostedBy = User.Identity.Name;

                        RSVP rsvp = new RSVP();
                        rsvp.AttendeeName = User.Identity.Name;
                        dinner.RSVPs.Add(rsvp);
                        _dinnerRepository.Save(dinner);
                        tx.Commit();
                        return RedirectToAction("Details", new { id = dinner.DinnerID });
                    }
                }
                catch {
                    ModelState.AddModelErrors(dinner.GetRuleViolations());
                }
            }

            return View(new DinnerFormViewModel(dinner));
        }

        //
        // HTTP GET: /Dinners/Delete/1

        [Authorize]
        public ActionResult Delete(int id) {
            using (var tx = _session.BeginTransaction())
            {
                Dinner dinner = _dinnerRepository.GetDinner(id);

                if (dinner == null)
                    return View("NotFound");

                if (!dinner.IsHostedBy(User.Identity.Name))
                    return View("InvalidOwner");
                return View(dinner);                
            }
        }

        // 
        // HTTP POST: /Dinners/Delete/1

        [AcceptVerbs(HttpVerbs.Post), Authorize]
        public ActionResult Delete(int id, string confirmButton) {
            using (var tx = _session.BeginTransaction())
            {
                Dinner dinner = _dinnerRepository.GetDinner(id);

                if (dinner == null)
                    return View("NotFound");

                if (!dinner.IsHostedBy(User.Identity.Name))
                    return View("InvalidOwner");

                _dinnerRepository.Delete(dinner);
                tx.Commit();
                return View("Deleted");
            }
        }
    }
}
