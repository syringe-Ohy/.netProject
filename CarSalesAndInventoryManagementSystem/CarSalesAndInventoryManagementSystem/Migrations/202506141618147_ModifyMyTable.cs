namespace CarSalesAndInventoryManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyMyTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderItems", "CarID", c => c.Int());
            AddColumn("dbo.OrderItems", "PartID", c => c.Int());
            AddColumn("dbo.OrderItems", "Order_OrderID", c => c.Int());
            CreateIndex("dbo.OrderItems", "CarID");
            CreateIndex("dbo.OrderItems", "PartID");
            CreateIndex("dbo.OrderItems", "Order_OrderID");
            AddForeignKey("dbo.OrderItems", "CarID", "dbo.Cars", "CarID");
            AddForeignKey("dbo.OrderItems", "PartID", "dbo.Parts", "PartID");
            AddForeignKey("dbo.OrderItems", "Order_OrderID", "dbo.Orders", "OrderID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderItems", "Order_OrderID", "dbo.Orders");
            DropForeignKey("dbo.OrderItems", "PartID", "dbo.Parts");
            DropForeignKey("dbo.OrderItems", "CarID", "dbo.Cars");
            DropIndex("dbo.OrderItems", new[] { "Order_OrderID" });
            DropIndex("dbo.OrderItems", new[] { "PartID" });
            DropIndex("dbo.OrderItems", new[] { "CarID" });
            DropColumn("dbo.OrderItems", "Order_OrderID");
            DropColumn("dbo.OrderItems", "PartID");
            DropColumn("dbo.OrderItems", "CarID");
        }
    }
}
