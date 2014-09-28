using PayMe.Core.Events;
using PayMe2.ViewModels.Abscenses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayMe2.Infrastructure;
using PayMe.Core;
using PayMe.Core.Eventprocessing;
using PayMe.Core.Entities;

namespace PayMe2.Controllers
{
    public class AbscenseController : Controller
    {
        // GET: Abscense
        public ActionResult Index(Guid id)
        {
            using (var db = Context.Create())
            {
                var abscenses = db.Abscenses.AsNoTracking().Where(a => a.InstanceId == id).ToList();
                var byPersons = abscenses.GroupBy(g => g.UserId).Select(g => new AbscensesByPerson
                {
                    UserId = g.Key,
                    Abscenses = g.OrderBy(x => x.From).ToList(),
                    Sum = g.Sum(x => (int)(x.To - x.From).TotalDays + 1)
                }).ToList();

                var persons = db.UserToInstanceMappings.Where(x => x.InstanceId == id).Select(x => x.User).ToDictionary(x => x.Id);
                foreach (var byPerson in byPersons)
                {
                    byPerson.User = persons[byPerson.UserId];
                }

                return View(new IndexViewModel
                {
                    InstanceId = id,
                    Persons = byPersons
                });
            }
        }

        public ActionResult Create(Guid id)
        {
            return View(new CreateViewModel
            {
                FromDate = DateTime.UtcNow.Date,
                ToDate = DateTime.UtcNow.Date,
            });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(Guid id, CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ev = AbscenseEventFactory.CreateAbscense(id, Guid.NewGuid(), model.Description, model.FromDate, model.ToDate, this.GetAudit());
                using (var db = Context.Create())
                {
                    db.StoredEvents.Add(StoredEvent.FromEvent(ev));
                    EventProcessor.Process(db, ev);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id });
                }
            }
            return View(model);
        }
    }
}