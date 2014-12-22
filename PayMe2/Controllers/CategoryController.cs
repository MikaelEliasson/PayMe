using PayMe.Core;
using PayMe.Core.Events;
using PayMe2.ViewModels.Categories;
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
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index(Guid instanceId)
        {
            using (var db = Context.Create())
            {
                var categories = db.Categories.AsNoTracking().Where(c => c.InstanceId == instanceId).OrderBy(c => c.Name).ToList();
                return View(new IndexViewModel
                {
                    InstanceId = instanceId,
                    Categories = categories,
                    Users = db.UserToInstanceMappings.Where(x => x.InstanceId == instanceId).Select(x => x.User).ToList().ToDictionary(x => x.Id)
                });
            }
        }

        public ActionResult Create(Guid instanceId)
        {
            using (var db = Context.Create())
            {
                return View(new CreateViewModel
                {
                    Users = GetUsersForListBox(instanceId, db),
                });
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(Guid instanceId, CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ev = CategoryEventFactory.CreateCategory(instanceId, Guid.NewGuid(), model.Name, model.DefaultUsers, model.Type, this.GetAudit());
                using (var db = Context.Create())
                {
                    db.StoredEvents.Add(StoredEvent.FromEvent(ev));
                    EventProcessor.Process(db, ev);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { instanceId });
                }
            }
            using (var db = Context.Create())
            {
                return View(new CreateViewModel
                {
                    Users = GetUsersForListBox(instanceId, db),
                });
            }
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