using PayMe.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayMe2.Infrastructure;
using PayMe.Core;
using PayMe.Core.Eventprocessing;
using PayMe.Core.Entities;
using PayMe2.ViewModels.Expenses;

namespace PayMe2.Controllers
{
    public class ExpenseController : Controller
    {
        // GET: Abscense
        public ActionResult Index(Guid instanceId)
        {
            using (var db = Context.Create())
            {
                var expenses = db.Expenses.AsNoTracking().Where(a => a.InstanceId == instanceId).ToList();

                var persons = db.UserToInstanceMappings.Where(x => x.InstanceId == instanceId).Select(x => x.User).ToDictionary(x => x.Id);
                var categories = db.Categories.Where(x => x.InstanceId == instanceId).ToDictionary(x => x.Id);

                return View(new IndexViewModel
                {
                    Expenses = expenses.OrderBy(e => e.Created).ToList(),
                    InstanceId = instanceId,
                    Persons = persons,
                    Categories = categories
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
                    Categories = db.Categories.AsNoTracking().Where(c => c.InstanceId == instanceId).Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }).ToList(),
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
                var ev = ExpenseEventFactory.CreateExpense(instanceId, Guid.NewGuid(), model.Category, model.Shop, model.Sum, model.Date, model.AffectedUsers, this.GetAudit());
                using (var db = Context.Create())
                {
                    db.StoredEvents.Add(StoredEvent.FromEvent(ev));
                    EventProcessor.Process(db, ev);
                    db.SaveChanges();
                    return RedirectToAction("Create", new { instanceId });
                }
            }
            return View(model);
        }

        public ActionResult CreateMany(Guid instanceId)
        {
            using (var db = Context.Create())
            {
                return View(new CreateManyViewModel
                {
                    InstanceId = instanceId,
                    Categories = db.Categories.AsNoTracking().Where(c => c.InstanceId == instanceId).ToList(),
                    Users = GetUsersForListBox(instanceId, db)
                });
            }
        }

        [HttpPost]
        public ActionResult CreateMany(Guid instanceId, IEnumerable<CreateManyDto> dtos, CreateViewModel model)
        {
            using (var db = Context.Create())
            {
                var expenses = new List<CreateExpenseEvent>();
                var audit = this.GetAudit();
                foreach (var item in dtos)
                {
                    expenses.Add(ExpenseEventFactory.CreateExpense(instanceId, Guid.NewGuid(), item.CategoryId, item.Shop, -item.Sum, item.Date, item.AffectedUsers, audit));
                }
                db.StoredEvents.AddRange(expenses.Select(e => StoredEvent.FromEvent(e)));
                foreach (var ev in expenses)
                {
                    EventProcessor.Process(db, ev);
                }
                db.SaveChanges();
                return RedirectToAction("Index", new { instanceId });
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