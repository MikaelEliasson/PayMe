using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PayMe.Core;
using System.Data.Entity;
using PayMe2.ViewModels.Instances;
using PayMe.Core.Events;
using PayMe2.Infrastructure;
using PayMe.Core.Entities;
using PayMe.Core.Eventprocessing;
using PayMe2.ViewModels;

namespace PayMe2.Controllers
{
    public class InstanceController : Controller
    {
        // GET: Instance
        public ActionResult Index()
        {
            using (var db = Context.Create())
            {
                var userId = Guid.Parse(User.Identity.GetUserId());
                var instances = db.UserToInstanceMappings.AsNoTracking().Where(m => m.UserId == userId).Select(x => x.Instance).ToList();
                return View(new ListViewModel
                {
                    Instances = instances
                });
            }
        }

        public ActionResult Details(Guid id)
        {
            using (var db = Context.Create())
            {
                var instance = db.Instances.Find(id);
                var users = db.UserToInstanceMappings.Where(m => m.InstanceId == id).Select(m => m.User).ToList();
                return View(new DetailsViewModel
                {
                    Instance = instance,
                    Users = users,
                    LastChanges = db.StoredEvents
                                        .AsNoTracking()
                                        .Where(e => e.InstanceId == id)
                                        .OrderByDescending(e => e.TimeUtc)
                                        .Select(EventForList.Transform)
                                        .ToList()
                });
            }
        }

        [InputPage]
        public ActionResult Create()
        {
            return View(new CreateViewModel()
            {
                StartDate = DateTime.Today
            });
        }

        [HttpPost]
        public ActionResult Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ev = InstanceEventFactory.CreateEvent(Guid.NewGuid(), model.Name, model.JoinCode, model.StartDate, model.EndDate, this.GetAudit());

                using (var db = Context.Create())
                {
                    db.StoredEvents.Add(StoredEvent.FromEvent(ev));
                    EventProcessor.Process(db, ev);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = ev.InstanceId.Value });
                }
            }
            return View(model);
        }

        [InputPage]
        public ActionResult Join(Guid id)
        {
            return View(new JoinViewModel(){});
        }

        [HttpPost]
        public ActionResult Join(Guid id, JoinViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ev = InstanceEventFactory.JoinEvent(id, this.GetAudit());

                using (var db = Context.Create())
                {
                    var ins = db.Instances.Find(id);
                    if (ins.JoinCode.Trim() != model.JoinCode.Trim())
                    {
                        ModelState.AddModelError("JoinCode", "The JoinCode was not correct");
                        return View(model);
                    }

                    db.StoredEvents.Add(StoredEvent.FromEvent(ev));
                    EventProcessor.Process(db, ev);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = ev.InstanceId.Value });
                }
            }
            return View(model);
        }
    }
}