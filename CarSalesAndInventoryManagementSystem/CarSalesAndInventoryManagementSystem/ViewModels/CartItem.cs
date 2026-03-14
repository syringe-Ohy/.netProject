using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarSalesAndInventoryManagementSystem.ViewModels
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        // "Car" or "Part" to distinguish between product types
        public string ProductType { get; set; }

        public decimal Subtotal
        {
            get { return Quantity * Price; }
        }
    }
}