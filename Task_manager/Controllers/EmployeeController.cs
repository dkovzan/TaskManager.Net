using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Task_manager.DAL;
using Task_manager.Models;

namespace Task_manager.Controllers
{
    public class EmployeeController : Controller
    {
        public ActionResult List()
        {
            var employees = new List<Employee>();
            using (var db = new TaskManagerContext())
            {
                employees = db.Employees.ToList();
            };
            return View(employees);
        }

        public ActionResult Edit(int id)
        {
            var employee = new Employee();
            if (id > 0)
            {
                using (var db = new TaskManagerContext())
                {
                    employee = db.Employees.Find(id);
                    if (employee == null)
                    {
                        return HttpNotFound();
                    }
                }
                return View(employee);
            }
            else if (id == 0)
            {
                return View(employee);
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult Delete(int id)
        {
            var employees = new List<Employee>();
            using (var db = new TaskManagerContext())
            {
                var employee = new Employee { Id = id };

                db.Employees.Attach(employee);
                db.Entry(employee).State = EntityState.Deleted;
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrUpdate([Bind(Include = "Id, FirstName, LastName, MiddleName, Position")]Employee employee)
        {
            var employees = new List<Employee>();
            using (var db = new TaskManagerContext())
            {
                db.Employees.AddOrUpdate(employee);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }

    }
}