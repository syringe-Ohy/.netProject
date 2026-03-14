using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarSalesAndInventoryManagementSystem.Models
{
    public class Part
    {
        [Key]
        public int PartID { get; set; }

        [Required]
        [StringLength(100)]
        public string PartName { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }  // Battery, Tyre, Brake, etc.

        [Required]
        [StringLength(50)]
        public string Origin { get; set; }  // Imported / Manufactured

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }
    }
}