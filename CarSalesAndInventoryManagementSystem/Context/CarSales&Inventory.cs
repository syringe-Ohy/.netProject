using CarSalesAndInventoryManagementSystem.Models;
using System;
using System.Data.Entity;
using System.Linq;

namespace CarSalesAndInventoryManagementSystem.Context
{
    public class CarSales_Inventory : DbContext
    {
        // Your context has been configured to use a 'CarSales_Inventory' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'CarSalesAndInventoryManagementSystem.Context.CarSales_Inventory' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'CarSales_Inventory' 
        // connection string in the application configuration file.
        public CarSales_Inventory()
            : base("name=CarSales&Inventory")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<TestDrive> TestDrives { get; set; }
        public DbSet<CarRequest> CarRequests { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}