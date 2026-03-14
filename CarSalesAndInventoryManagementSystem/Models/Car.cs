using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarSalesAndInventoryManagementSystem.Models
{
    public class Car
    {
        [Key]
        public int CarID { get; set; }

        [Required]
        [StringLength(100)]
        public string Model { get; set; }

        [Required]
        [StringLength(100)]
        public string Brand { get; set; }

        [Required]
        [StringLength(50)]
        public string EngineType { get; set; } // e.g., Petrol, Diesel, Electric

        [Required]
        public decimal Mileage { get; set; } // e.g., km per liter

        [Required]
        public decimal TankCapacity { get; set; } // in liters

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }

        [Display(Name = "Image URL")]
        [StringLength(1024)] // URLs
        public string ImageUrl { get; set; }
    }
}