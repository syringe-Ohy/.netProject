using System;

using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace CarSalesAndInventoryManagementSystem.Models

{

    public class CarRequest

    {

        [Key]

        public int CarRequestID { get; set; }

        [ForeignKey("User")]

        public int UserID { get; set; }

        public virtual User User { get; set; }

        [Required]

        [StringLength(100)]

        public string Brand { get; set; }

        [Required]

        [StringLength(100)]

        public string Model { get; set; }

        [StringLength(50)]

        [Display(Name = "Engine Type (Optional)")]

        public string EngineType { get; set; }

        [Required]


        [Column(TypeName = "datetime2")]

        public DateTime RequestDate { get; set; }

        [Required]

        [StringLength(50)]

        public string Status { get; set; } // e.g., "New", "Contacted", "Resolved"

        [DataType(DataType.MultilineText)]

        [Display(Name = "Additional Notes (Optional)")]

        [StringLength(500)]

        public string Notes { get; set; }

    }

}

