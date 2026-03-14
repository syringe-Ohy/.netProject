using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarSalesAndInventoryManagementSystem.Models
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }

        [ForeignKey("Order")]
        public int OrderID { get; set; }
        public Order Order { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(50)]
        public string Method { get; set; }  //  Credit Card, UPI, etc.

        [Required]
        public DateTime Date { get; set; }
    }
}