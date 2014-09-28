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
        public ActionResult Index(Guid id)
        {
            using (var db = Context.Create())
            {
                var categories = db.Categories.AsNoTracking().Where(c => c.InstanceId == id).OrderBy(c => c.Name).ToList();
                return View(new IndexViewModel
                {
                    InstanceId = id,
                    Categories = categories,
                    Users = db.UserToInstanceMappings.Where(x => x.InstanceId == id).Select(x => x.User).ToList().ToDictionary(x => x.Id)
                });
            }
        }

        public ActionResult Create(Guid id)
        {
            using (var db = Context.Create())
            {
                return View(new CreateViewModel
                {
                    Users = GetUsersForListBox(id, db),
                });
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(Guid id, CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ev = CategoryEventFactory.CreateCategory(id, Guid.NewGuid(), model.Name, model.DefaultUsers, model.Type, this.GetAudit());
                using (var db = Context.Create())
                {
                    db.StoredEvents.Add(StoredEvent.FromEvent(ev));
                    EventProcessor.Process(db, ev);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id });
                }
            }
            using (var db = Context.Create())
            {
                return View(new CreateViewModel
                {
                    Users = GetUsersForListBox(id, db),
                });
            }
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