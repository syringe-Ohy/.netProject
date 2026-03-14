using System.Data.Entity;

using System.Linq;

using System.Net;

using System.Web.Mvc;

using CarSalesAndInventoryManagementSystem.Context;

using CarSalesAndInventoryManagementSystem.Models;

namespace CarSalesAndInventoryManagementSystem.Controllers

{

    public class CarRequestController : Controller

    {

        private readonly CarSales_Inventory _dbContext;

        public CarRequestController()

        {

            _dbContext = new CarSales_Inventory();

        }

        /// <summary>

        /// A filter that runs before any action to ensure the user is logged in.

        /// </summary>

        protected override void OnActionExecuting(ActionExecutingContext filterContext)

        {

            if (Session["UserID"] == null)

            {

                filterContext.Result = RedirectToAction("Login", "User");

                return;

            }

            base.OnActionExecuting(filterContext);

        }

        // GET: /CarRequest/MyCarRequests

        // Shows a list of requests made by the current user.

        public ActionResult MyCarRequests()

        {

            var userId = (int)Session["UserID"];

            var myRequests = _dbContext.CarRequests

                .Where(r => r.UserID == userId)

                .OrderByDescending(r => r.RequestDate)

                .ToList();

            return View(myRequests);

        }

        // GET: /CarRequest/Edit/{id}

        // Shows the form to edit a request.

        public ActionResult Edit(int? id)

        {

            if (id == null)

            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            CarRequest carRequest = _dbContext.CarRequests.Find(id);

            if (carRequest == null)

            {

                return HttpNotFound();

            }

            // --- SECURITY CHECK ---

            // Ensure the logged-in user is the one who created the request.

            var userId = (int)Session["UserID"];

            if (carRequest.UserID != userId)

            {

                // If not the owner, forbid access.

                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            }

            return View(carRequest);

        }

        // POST: /CarRequest/Edit/{id}

        // Processes the submitted changes.

        [HttpPost]

        [ValidateAntiForgeryToken]

        public ActionResult Edit(CarRequest carRequest)

        {

            // --- SECURITY CHECK ---

            var userId = (int)Session["UserID"];

            var originalRequest = _dbContext.CarRequests.AsNoTracking().FirstOrDefault(r => r.CarRequestID == carRequest.CarRequestID);

            if (originalRequest == null || originalRequest.UserID != userId)

            {

                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            }

            // Only allow editing if the status is "New"

            if (originalRequest.Status != "New")

            {

                ModelState.AddModelError("", "This request can no longer be edited as it is already being processed.");

                return View(carRequest);

            }

            if (ModelState.IsValid)

            {

                // Set the state to modified and update only allowed fields

                var entry = _dbContext.Entry(carRequest);

                entry.State = EntityState.Modified;

                // Prevent crucial data from being changed by the form post

                entry.Property(e => e.UserID).IsModified = false;

                entry.Property(e => e.RequestDate).IsModified = false;

                entry.Property(e => e.Status).IsModified = false;

                _dbContext.SaveChanges();

                TempData["SuccessMessage"] = "Your request has been updated successfully.";

                return RedirectToAction("MyCarRequests");

            }

            return View(carRequest);

        }

        // GET: /CarRequest/Delete/{id}

        // Shows the delete confirmation page.

        public ActionResult Delete(int? id)

        {

            if (id == null)

            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            CarRequest carRequest = _dbContext.CarRequests.Find(id);

            if (carRequest == null)

            {

                return HttpNotFound();

            }

            // --- SECURITY CHECK ---

            var userId = (int)Session["UserID"];

            if (carRequest.UserID != userId)

            {

                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            }

            return View(carRequest);

        }

        // POST: /CarRequest/Delete/{id}

        // Deletes the request.

        [HttpPost, ActionName("Delete")]

        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)

        {

            CarRequest carRequest = _dbContext.CarRequests.Find(id);

            // --- SECURITY CHECK ---

            var userId = (int)Session["UserID"];

            if (carRequest == null || carRequest.UserID != userId)

            {

                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            }

            _dbContext.CarRequests.Remove(carRequest);

            _dbContext.SaveChanges();

            TempData["SuccessMessage"] = "Your request has been successfully deleted.";

            return RedirectToAction("MyCarRequests");

        }

    }

}

