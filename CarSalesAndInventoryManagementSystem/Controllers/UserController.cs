using System;
using System.Linq;
using System.Web.Mvc;
using CarSalesAndInventoryManagementSystem.Context;
using CarSalesAndInventoryManagementSystem.Models;

namespace CarSalesAndInventoryManagementSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly CarSales_Inventory _dbContext;

        public UserController()
        {
            this._dbContext = new CarSales_Inventory();
        }

        //VIEW ALL USERS (ADMIN ONLY)
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["Role"] as string != "Admin")
            {
                if (Session["Role"] as string == "Customer")
                {
                    return RedirectToAction("Dashboard", "Customer");
                }
                return RedirectToAction("Login", "User");
            }

            var allUsers = _dbContext.Users.ToList(); // Make sure _dbContext is initialized
            return View(allUsers);
        }
     

        //  LOGIN 
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["UserID"] != null)
            {
                if (Session["Role"] as string == "Admin")
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                return RedirectToAction("Dashboard", "Customer");
            }
            return View();
        }

        [HttpPost]

        public ActionResult Login(User loginAttempt)
        {
         
            ModelState.Remove("Role");
            ModelState.Remove("UserID");
            ModelState.Remove("Email");
            ModelState.Remove("DOB");

            if (ModelState.IsValid)
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.Name == loginAttempt.Name && u.Password == loginAttempt.Password);

                if (user != null)
                {
                    Session["UserID"] = user.UserID;
                    Session["UserName"] = user.Name;
                    Session["Role"] = user.Role;

                    if (user.Role == "Admin")
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else if (user.Role == "Customer")
                    {
                        return RedirectToAction("Dashboard", "Customer");
                    }
                }

                ViewBag.ErrorMessage = "Invalid username or password.";
                return View(loginAttempt);
            }

            return View(loginAttempt);
        }

        //  LOGOUT  
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            TempData["LogoutMessage"] = "You have been successfully logged out.";
            return RedirectToAction("Login", "User");
        }

        //  SIGN UP
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        public ActionResult MyOrders()
        {
            int userId = (int)Session["UserID"]; // Get logged-in user's ID from session

            var orders = _dbContext.Orders
                            .Where(o => o.UserID == userId)
                            .OrderByDescending(o => o.OrderDate)
                            .ToList();

            return View(orders);
        }


        [HttpPost]
     
        public ActionResult SignUp(User newUser)
        {
            ModelState.Remove("Role");

            if (ModelState.IsValid)
            {
                if (_dbContext.Users.Any(u => u.Name == newUser.Name))
                {
                    ModelState.AddModelError("Name", "This username is already taken. Please choose another.");
                    return View(newUser);
                }
                if (_dbContext.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "This email address is already registered.");
                    return View(newUser);
                }

                newUser.Role = "Customer";

                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();

                TempData["SuccessMessage"] = "Registration successful! Please log in.";
                return RedirectToAction("Login");
            }

            return View(newUser);
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
