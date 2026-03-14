using CarSalesAndInventoryManagementSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class OrderItem
{
    [Key]
    public int OrderItemID { get; set; } // NEW primary key

    [ForeignKey("Order")]
    public int OrderID { get; set; }     // Just a foreign key now
    public virtual Order Order { get; set; }

    [ForeignKey("Car")]
    public int? CarID { get; set; }
    public virtual Car Car { get; set; }

    [ForeignKey("Part")]
    public int? PartID { get; set; }
    public virtual Part Part { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }
}
