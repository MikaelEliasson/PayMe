using PayMe.Core;
using PayMe.Core.Events;
using PayMe2.ViewModels.Transfers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayMe2.Infrastructure;
using PayMe.Core.Entities;
using PayMe.Core.Eventprocessing;

namespace PayMe2.Controllers
{
    public class TransferController : Controller
    {
        // GET: Abscense
        public ActionResult Index(Guid id)
        {
            using (var db = Context.Create())
            {
                var transfers = db.Transfers.AsNoTracking().Where(t => t.InstanceId == id).ToList();

                var persons = db.UserToInstanceMappings.Where(x => x.InstanceId == id).Select(x => x.User).ToDictionary(x => x.Id);

                return View(new IndexViewModel
                {
                    Transfers = transfers.OrderBy(e => e.Date).ToList(),
                    InstanceId = id,
                    Persons = persons
                });
            }
        }

        public ActionResult Create(Guid id)
        {
            using (var db = Context.Create())
            {
                return View(new CreateViewModel
                {
                    InstanceId = id,
                    Date = DateTime.Today,
                    Users = GetUsersForListBox(id, db)
                });
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(Guid id, CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ev = TransferEventFactory.CreateTransfer(id, Guid.NewGuid(), model.ToUserId , model.Sum, model.Date, this.GetAudit());
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

        private static List<SelectListItem> GetUsersForListBox(Guid id, Context db)
        {
            return db.UserToInstanceMappings.AsNoTracking().Where(x => x.InstanceId == id).Select(x => x.User).OrderBy(x => x.FirstName).Select(x => new SelectListItem
            {
                Text = x.FirstName + " " + x.LastName,
                Value = x.Id.ToString()
            }).ToList();
        }
    }
}