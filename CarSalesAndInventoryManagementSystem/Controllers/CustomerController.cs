using System;
using System.Linq;
using System.Web.Mvc;
using CarSalesAndInventoryManagementSystem.Context;
using CarSalesAndInventoryManagementSystem.Models;
using System.Data.Entity;
using System.Collections.Generic;


namespace CarSalesAndInventoryManagementSystem.Controllers
{

    public class CustomerController : Controller
    {
        private readonly CarSales_Inventory _dbContext;

        public CustomerController()
        {
            _dbContext = new CarSales_Inventory();
        }
        // GET: Customer/Dashboard
        public ActionResult Dashboard()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            return View();
        }
     
    

        public ActionResult OrderDetails(int id)
        {
            var order = _dbContext.Orders
                .Include("User")
                .Include("OrderItems.Car")
                .Include("OrderItems.Part")
                .FirstOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }
        public ActionResult CancelOrder(int id)
        {
            var order = _dbContext.Orders.Include("OrderItems").Include("Payments").FirstOrDefault(o => o.OrderID == id);
            if (order == null)
            {
                return HttpNotFound();
            }

            // Update stock: return items to inventory
            foreach (var item in order.OrderItems.ToList())
            {
                if (item.CarID != null)
                {
                    var car = _dbContext.Cars.FirstOrDefault(c => c.CarID == item.CarID);
                    if (car != null)
                    {
                        car.Stock+= item.Quantity;
                    }
                }
                else if (item.PartID != null)
                {
                    var part = _dbContext.Parts.FirstOrDefault(p => p.PartID == item.PartID);
                    if (part != null)
                    {
                        part.Stock += item.Quantity;
                    }
                }

                // Remove the order item
                _dbContext.OrderItems.Remove(item);
            }

            // Remove any payment records (optional)
            // foreach (var payment in order.Payments?.ToList() ?? new List<Payment>())
            //{
            ///    db.Payments.Remove(payment);
        //}

        // Remove the order itself
        _dbContext.Orders.Remove(order);
            _dbContext.SaveChanges();

            TempData["SuccessMessage"] = "Order cancelled and stock updated.";
            return RedirectToAction("MyOrders");
        }



        public ActionResult BrowseCars()
        {
            // show cars that are in stock.
            var availableCars = _dbContext.Cars.Where(c => c.Stock > 0).ToList();
            return View(availableCars);
        }


        // GET: Customer/CarDetails/{id}
        public ActionResult CarDetails(int id)
        {
            var car = _dbContext.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // GET: Customer/BrowseParts
       
        public ActionResult BrowseParts()
        {
            var availableParts = _dbContext.Parts.Where(p => p.Stock > 0).ToList();
            return View(availableParts);
        }
        // GET: Customer/PartDetails/{id}


        public ActionResult PartDetails(int id)
        {
            var part = _dbContext.Parts.Find(id);
            if (part == null)
            {
                return HttpNotFound();
            }
            return View(part);
        }

    
        // GET: Customer/MyOrders
        
        public ActionResult MyOrders()
        {

            var userId = (int)Session["UserID"];
            var myOrders = _dbContext.Orders
                .Where(o => o.UserID == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(myOrders);
        }
        public ActionResult RequestCar()
        {
            return View();
        }
    }
}