using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarSalesAndInventoryManagementSystem.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }  // Pending, Paid

        [Required]
        public decimal TotalAmount { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}