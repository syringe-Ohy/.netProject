using System;
using System.Data.Entity; 
using System.Linq;
using System.Web.Mvc;
using CarSalesAndInventoryManagementSystem.Context;
using CarSalesAndInventoryManagementSystem.Models;
using System.Collections.Generic;

namespace CarSalesAndInventoryManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly CarSales_Inventory _dbContext;

        public AdminController()
        {
            _dbContext = new CarSales_Inventory();
        }

        //  DASHBOARD
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["Role"] as string != "Admin")
            {
                filterContext.Result = RedirectToAction("Login", "User");
            }
            base.OnActionExecuting(filterContext);
        }

        public ActionResult Dashboard()
        {
            return View();
        }

       //  USER MANAGEMENT 

        [HttpGet]
        public ActionResult AddAdmin()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var user = _dbContext.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
            TempData["SuccessMessage"] = "User deleted successfully.";
            return RedirectToAction("Index", "User");

        }




        [HttpPost]
        public ActionResult AddAdmin(User newUser)
        {
            if (ModelState.IsValid)
            {
                if (_dbContext.Users.Any(u => u.Name == newUser.Name))
                {
                    ModelState.AddModelError("Name", "Username already exists.");
                    return View(newUser);
                }
                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();
                TempData["SuccessMessage"] = $"User '{newUser.Name}' created successfully.";
                return RedirectToAction("Index", "User"); 
            }
            return View(newUser);
        }

        // CAR INVENTORY MANAGEMENT (CRUD)

        public ActionResult CarInventory()
        {
            var cars = _dbContext.Cars.ToList();
            return View(cars);
        }

        [HttpGet]
        public ActionResult AddCar() => View();

        [HttpPost]
        public ActionResult AddCar(Car car)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Cars.Add(car);
                _dbContext.SaveChanges();
                return RedirectToAction("CarInventory");
            }
            return View(car);
        }

        [HttpGet]
        public ActionResult EditCar(int id)
        {
            var car = _dbContext.Cars.Find(id);
            if (car == null) return HttpNotFound();
            return View(car);
        }

        [HttpPost]
        public ActionResult EditCar(Car car)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(car).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("CarInventory");
            }
            return View(car);
        }

        [HttpGet]
        public ActionResult DeleteCar(int id)
        {
            var car = _dbContext.Cars.Find(id);
            if (car == null) return HttpNotFound();
            return View(car);
        }

        [HttpPost, ActionName("DeleteCar")]
        public ActionResult DeleteCarConfirmed(int id)
        {
            var car = _dbContext.Cars.Find(id);
            _dbContext.Cars.Remove(car);
            _dbContext.SaveChanges();
            return RedirectToAction("CarInventory");
        }
        public ActionResult CarDetails(int id)
        {
            var car = _dbContext.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        //PART INVENTORY MANAGEMENT (CRUD)

        public ActionResult PartInventory()
        {
            var parts = _dbContext.Parts.ToList();
            return View(parts);
        }

        [HttpGet]
        public ActionResult AddPart() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPart(Part part)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Parts.Add(part);
                _dbContext.SaveChanges();
                return RedirectToAction("PartInventory");
            }
            return View(part);
        }

        [HttpGet]
        public ActionResult EditPart(int id)
        {
            var part = _dbContext.Parts.Find(id);
            if (part == null) return HttpNotFound();
            return View(part);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPart(Part part)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(part).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("PartInventory");
            }
            return View(part);
        }

        // ORDER MANAGEMENT 
        public ActionResult ManageOrders()
        {
            var orders = _dbContext.Orders.Include(o => o.User).OrderByDescending(o => o.OrderDate).ToList();
            return View(orders);
        }

        public ActionResult OrderDetails(int id)
        {
            var order = _dbContext.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems.Select(oi => oi.Car))
                .Include(o => o.OrderItems.Select(oi => oi.Part))
                .FirstOrDefault(o => o.OrderID == id);

            if (order == null) return HttpNotFound();
            return View(order);
        }
        public ActionResult GDetails(int id)
        {
            var order = _dbContext.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems.Select(oi => oi.Car))
                .Include(o => o.OrderItems.Select(oi => oi.Part))
                .FirstOrDefault(o => o.OrderID == id);

            if (order == null) return HttpNotFound();
            return View(order);
        }

        //  TEST DRIVE MANAGEMENT 

        public ActionResult ManageTestDrives()
        {
            var testDrives = _dbContext.TestDrives.Include(td => td.User).Include(td => td.Car).OrderByDescending(td => td.DateRequested).ToList();
            return View(testDrives);
        }

        [HttpPost]
        public ActionResult UpdateTestDriveStatus(int testDriveId, string status)
        {
            var testDrive = _dbContext.TestDrives.Find(testDriveId);
            if (testDrive != null)
            {
                testDrive.Status = status;
                _dbContext.SaveChanges();
            }
            return RedirectToAction("ManageTestDrives");
        }

        //  REPORTING 

        [HttpGet]
        public ActionResult Reports()
        {
            return View();
        }
        [HttpPost]
     

        public ActionResult UpdateStatus(int orderId, string status)
        {
            var order = _dbContext.Orders.Find(orderId);
            if (order != null)
            {
                order.Status = status;
                _dbContext.SaveChanges();
            }

            // Redirect back to the order management page
            return RedirectToAction("ManageOrders");
        }
        // for EF6; or Microsoft.EntityFrameworkCore for EF Core

[HttpPost]
    
    public ActionResult GenerateSalesReport(DateTime startDate, DateTime endDate)
    {
        var confirmedOrders = _dbContext.Orders
            .Include(o => o.User)  // eagerly load User navigation property
            .Where(o => o.Status.Equals("confirmed", StringComparison.OrdinalIgnoreCase)
                        && o.OrderDate >= startDate
                        && o.OrderDate <= endDate)
            .OrderBy(o => o.OrderDate)
            .ToList();

        return View("SalesReport", confirmedOrders);
    }



    /// GET: /Admin/ManageCarRequests
    public ActionResult ManageCarRequests()
        {
            var requests = _dbContext.CarRequests
                .Include(r => r.User) // Include user details to show who made the request
                .OrderByDescending(r => r.RequestDate)
                .ToList();

            return View(requests);
        }

        [HttpPost]

        public ActionResult UpdateRequestStatus(int carRequestId, string status)
        {
            var request = _dbContext.CarRequests.Find(carRequestId);
            if (request != null)
            {
                request.Status = status;
                _dbContext.SaveChanges();
            }
            return RedirectToAction("ManageCarRequests");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
