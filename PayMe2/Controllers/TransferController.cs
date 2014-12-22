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
        public ActionResult Index(Guid instanceId)
        {
            using (var db = Context.Create())
            {
                var transfers = db.Transfers.AsNoTracking().Where(t => t.InstanceId == instanceId).ToList();

                var persons = db.UserToInstanceMappings.Where(x => x.InstanceId == instanceId).Select(x => x.User).ToDictionary(x => x.Id);

                return View(new IndexViewModel
                {
                    Transfers = transfers.OrderBy(e => e.Date).ToList(),
                    InstanceId = instanceId,
                    Persons = persons
                });
            }
        }

        public ActionResult Create(Guid instanceId)
        {
            using (var db = Context.Create())
            {
                return View(new CreateViewModel
                {
                    InstanceId = instanceId,
                    Date = DateTime.Today,
                    Users = GetUsersForListBox(instanceId, db)
                });
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(Guid instanceId, CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ev = TransferEventFactory.CreateTransfer(instanceId, Guid.NewGuid(), model.ToUserId , model.Sum, model.Date, this.GetAudit());
                using (var db = Context.Create())
                {
                    db.StoredEvents.Add(StoredEvent.FromEvent(ev));
                    EventProcessor.Process(db, ev);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { instanceId = instanceId });
                }
            }
            return View(model);
        }

        private static List<SelectListItem> GetUsersForListBox(Guid instanceId, Context db)
        {
            return db.UserToInstanceMappings.AsNoTracking().Where(x => x.InstanceId == instanceId).Select(x => x.User).OrderBy(x => x.FirstName).Select(x => new SelectListItem
            {
                Text = x.FirstName + " " + x.LastName,
                Value = x.Id.ToString()
            }).ToList();
        }
    }
}