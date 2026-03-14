using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CarSalesAndInventoryManagementSystem.Context;
using CarSalesAndInventoryManagementSystem.Models;
using CarSalesAndInventoryManagementSystem.ViewModels;

namespace CarSalesAndInventoryManagementSystem.Controllers
{
    public class CartController : Controller
    {
        private readonly CarSales_Inventory _dbContext;

        public CartController()
        {
            _dbContext = new CarSales_Inventory();
        }

        // Use session UserName for cart key (fallback "Guest" if null)
        private string CartKey => $"Cart_{Session["UserName"] ?? "Guest"}";

        // Ensure user is logged in before any action except AddToCart (optional)
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Allow AddToCart for guest users if you want
            if (filterContext.ActionDescriptor.ActionName == "AddToCart")
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            if (Session["UserID"] == null)
            {
                filterContext.Result = RedirectToAction("Login", "User");
                return;
            }

            base.OnActionExecuting(filterContext);
        }

        // GET: /Cart/Index
        public ActionResult Index()
        {
            var cart = Session[CartKey] as List<CartItem> ?? new List<CartItem>();

            if (!cart.Any())
            {
                ViewBag.Message = "Your shopping cart is empty.";
            }

            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateQuantity(int productId, string productType, int quantity)
        {
            if (quantity < 1)
            {
                TempData["ErrorMessage"] = "Quantity must be at least 1.";
                return RedirectToAction("Index");
            }

            var cart = Session[CartKey] as List<CartItem>;
            if (cart == null)
            {
                TempData["ErrorMessage"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }

            var item = cart.FirstOrDefault(c => c.ProductId == productId && c.ProductType == productType);
            if (item != null)
            {
                item.Quantity = quantity;
                Session[CartKey] = cart; // Save changes
                TempData["SuccessMessage"] = "Quantity updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Item not found in your cart.";
            }

            return RedirectToAction("Index");
        }

        // POST: /Cart/AddToCart
        [HttpPost]
        public ActionResult AddToCart(int id, string productType)
        {
            // ✅ Check if user is logged in
            if (Session["UserID"] == null)
            {
                TempData["ErrorMessage"] = "Please log in to add items to your cart.";
                return RedirectToAction("Login", "User");
            }

            var cart = Session[CartKey] as List<CartItem> ?? new List<CartItem>();

            var existingItem = cart.FirstOrDefault(item => item.ProductId == id && item.ProductType == productType);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                if (productType == "Car")
                {
                    var car = _dbContext.Cars.Find(id);
                    if (car != null && car.Stock > 0)
                    {
                        cart.Add(new CartItem
                        {
                            ProductId = car.CarID,
                            ProductName = car.Brand + " " + car.Model,
                            Quantity = 1,
                            Price = car.Price,
                            ImageUrl = car.ImageUrl,
                            ProductType = "Car"
                        });
                    }
                }
                else if (productType == "Part")
                {
                    var part = _dbContext.Parts.Find(id);
                    if (part != null && part.Stock > 0)
                    {
                        cart.Add(new CartItem
                        {
                            ProductId = part.PartID,
                            ProductName = part.PartName,
                            Quantity = 1,
                            Price = part.Price,
                            ProductType = "Part"
                        });
                    }
                }
            }

            Session[CartKey] = cart;
            TempData["SuccessMessage"] = "Item added to cart successfully!";
            return Redirect(Request.UrlReferrer.ToString());
        }


        // POST: /Cart/RemoveFromCart
        [HttpPost]
        public ActionResult RemoveFromCart(int productId, string productType)
        {
            var cart = Session[CartKey] as List<CartItem> ?? new List<CartItem>();
            var itemToRemove = cart.FirstOrDefault(item => item.ProductId == productId && item.ProductType == productType);

            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
            }

            Session[CartKey] = cart;
            return RedirectToAction("Index");
        }

        // PartialView for the navbar cart summary
        public ActionResult CartSummary()
        {
            var cart = Session[CartKey] as List<CartItem> ?? new List<CartItem>();
            ViewBag.ItemCount = cart.Sum(item => item.Quantity);
            return PartialView("_CartSummary");
        }

        // GET: /Cart/Checkout
        public ActionResult Checkout()
        {
            var cart = Session[CartKey] as List<CartItem> ?? new List<CartItem>();
            if (!cart.Any())
            {
                TempData["ErrorMessage"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }

            return View(cart);
        }

        // POST: /Cart/PlaceOrder
        [HttpPost]
        public ActionResult PlaceOrder()
        {
            var cart = Session[CartKey] as List<CartItem> ?? new List<CartItem>();
            if (!cart.Any())
            {
                TempData["ErrorMessage"] = "Cart is empty.";
                return RedirectToAction("Checkout");
            }

            var userId = Session["UserID"] as int?;
            if (userId == null)
            {
                TempData["ErrorMessage"] = "Please log in to place an order.";
                return RedirectToAction("Login", "User");
            }

            var user = _dbContext.Users.Find(userId.Value);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Login", "User");
            }

            var order = new Order
            {
                UserID = user.UserID,
                OrderDate = DateTime.Now,
                Status = "Pending",
                TotalAmount = cart.Sum(i => i.Price * i.Quantity),
                OrderItems = new List<OrderItem>()
            };

            foreach (var item in cart)
            {
                order.OrderItems.Add(new OrderItem
                {
                    CarID = item.ProductType == "Car" ? (int?)item.ProductId : null,
                    PartID = item.ProductType == "Part" ? (int?)item.ProductId : null,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price
                });

                // Decrease stock
                if (item.ProductType == "Car")
                {
                    var car = _dbContext.Cars.Find(item.ProductId);
                    if (car != null) car.Stock -= item.Quantity;
                }
                else if (item.ProductType == "Part")
                {
                    var part = _dbContext.Parts.Find(item.ProductId);
                    if (part != null) part.Stock -= item.Quantity;
                }
            }

            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();

            // Clear cart
            Session[CartKey] = new List<CartItem>();

            TempData["SuccessMessage"] = "Order placed successfully!";
            return RedirectToAction("OrderConfirmation", new { id = order.OrderID });
        }

        public ActionResult OrderConfirmation(int id)
        {
            var order = _dbContext.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
    }
}
