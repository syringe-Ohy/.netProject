using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarSalesAndInventoryManagementSystem.Models
{
    public class TestDrive
    {
        [Key]
        public int TestDriveID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; }

        [ForeignKey("Car")]
        public int CarID { get; set; }
        public Car Car { get; set; }

        [Required]
        public DateTime DateRequested { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }
    }
}