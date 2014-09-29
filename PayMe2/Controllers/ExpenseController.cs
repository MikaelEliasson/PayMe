﻿using PayMe.Core.Events;
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
        public ActionResult Index(Guid id)
        {
            using (var db = Context.Create())
            {
                var expenses = db.Expenses.AsNoTracking().Where(a => a.InstanceId == id).ToList();

                var persons = db.UserToInstanceMappings.Where(x => x.InstanceId == id).Select(x => x.User).ToDictionary(x => x.Id);
                var categories = db.Categories.Where(x => x.InstanceId == id).ToDictionary(x => x.Id);

                return View(new IndexViewModel
                {
                    Expenses = expenses.OrderBy(e => e.Created).ToList(),
                    InstanceId = id,
                    Persons = persons,
                    Categories = categories
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
                    Categories = db.Categories.AsNoTracking().Where(c => c.InstanceId == id).Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }).ToList(),
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
                var ev = ExpenseEventFactory.CreateExpense(id, Guid.NewGuid(), model.Category, model.Shop, model.Sum, model.Date, model.AffectedUsers, this.GetAudit());
                using (var db = Context.Create())
                {
                    db.StoredEvents.Add(StoredEvent.FromEvent(ev));
                    EventProcessor.Process(db, ev);
                    db.SaveChanges();
                    return RedirectToAction("Create", new { id });
                }
            }
            return View(model);
        }

        public ActionResult CreateMany(Guid id)
        {
            using (var db = Context.Create())
            {
                return View(new CreateManyViewModel
                {
                    InstanceId = id,
                    Categories = db.Categories.AsNoTracking().Where(c => c.InstanceId == id).ToList(),
                    Users = GetUsersForListBox(id, db)
                });
            }
        }

        [HttpPost]
        public ActionResult CreateMany(Guid id, IEnumerable<CreateManyDto> dtos, CreateViewModel model)
        {
            using (var db = Context.Create())
            {
                var expenses = new List<CreateExpenseEvent>();
                var audit = this.GetAudit();
                foreach (var item in dtos)
                {
                    expenses.Add(ExpenseEventFactory.CreateExpense(id, Guid.NewGuid(), item.CategoryId, item.Shop, -item.Sum, item.Date, item.AffectedUsers, audit));
                }
                db.StoredEvents.AddRange(expenses.Select(e => StoredEvent.FromEvent(e)));
                foreach (var ev in expenses)
                {
                    EventProcessor.Process(db, ev);
                }
                db.SaveChanges();
                return RedirectToAction("Index", new { id = id });
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