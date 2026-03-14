using System;

using System.Linq;

using System.Web.Mvc;

using CarSalesAndInventoryManagementSystem.Context;

using CarSalesAndInventoryManagementSystem.Models;

using System.Net;

namespace CarSalesAndInventoryManagementSystem.Controllers

{

    public class TestDriveController : Controller

    {

        private readonly CarSales_Inventory _dbContext;

        public TestDriveController()

        {

            _dbContext = new CarSales_Inventory();

        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)

        {

            if (Session["UserID"] == null)

            {

                filterContext.Result = RedirectToAction("Login", "User");

                return;

            }

            base.OnActionExecuting(filterContext);

        }

        [HttpGet]

        public ActionResult Create(int? carId)

        {

            var testDrive = new TestDrive

            {

                DateRequested = DateTime.Now.AddDays(1)

            };

            var cars = _dbContext.Cars

                .Where(c => c.Stock > 0)

                .Select(c => new {

                    CarID = c.CarID,

                    DisplayText = c.Brand + " " + c.Model

                }).ToList();

            ViewBag.CarList = new SelectList(cars, "CarID", "DisplayText");

            if (carId.HasValue)

            {

                testDrive.CarID = carId.Value;

            }

            return View(testDrive);

        }

        [HttpPost]

        [ValidateAntiForgeryToken]

        public ActionResult Create(TestDrive testDrive)

        {

            ModelState.Remove("Status");

            if (ModelState.IsValid)

            {

                testDrive.UserID = (int)Session["UserID"];

                testDrive.Status = "Requested";

                _dbContext.TestDrives.Add(testDrive);

                _dbContext.SaveChanges();

                TempData["SuccessMessage"] = "Your test drive has been successfully booked! We will contact you shortly to confirm.";

                return RedirectToAction("MyTestDrives");

            }

            var cars = _dbContext.Cars

                .Where(c => c.Stock > 0)

                .Select(c => new {

                    CarID = c.CarID,

                    DisplayText = c.Brand + " " + c.Model

                }).ToList();

            ViewBag.CarList = new SelectList(cars, "CarID", "DisplayText", testDrive.CarID);

            return View(testDrive);

        }

        public ActionResult MyTestDrives()

        {

            var userId = (int)Session["UserID"];

            var myBookings = _dbContext.TestDrives

                .Include("Car")

                .Where(td => td.UserID == userId)

                .OrderByDescending(td => td.DateRequested)

                .ToList();

            return View(myBookings);

        }

        // --- NEW ACTION TO CANCEL A BOOKING ---

        [HttpPost]

        [ValidateAntiForgeryToken]

        public ActionResult Cancel(int id)

        {

            var testDrive = _dbContext.TestDrives.Find(id);

            if (testDrive == null)

            {

                return HttpNotFound();

            }

            // Security Check: Ensure the logged-in user owns this booking

            var userId = (int)Session["UserID"];

            if (testDrive.UserID != userId)

            {

                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            }

            // Only allow cancellation if the request is still pending

            if (testDrive.Status == "Requested")

            {

                testDrive.Status = "Cancelled";

                _dbContext.SaveChanges();

                TempData["SuccessMessage"] = "Your test drive booking has been successfully cancelled.";

            }

            else

            {

                TempData["ErrorMessage"] = "This booking can no longer be cancelled as it has already been processed.";

            }

            return RedirectToAction("MyTestDrives");

        }

    }

}

