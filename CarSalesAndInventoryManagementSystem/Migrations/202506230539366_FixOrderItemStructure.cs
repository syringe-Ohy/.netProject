namespace CarSalesAndInventoryManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class FixOrderItemStructure : DbMigration
    {
        public override void Up()
        {
            // Drop foreign keys that reference columns we want to modify
            DropForeignKey("dbo.OrderItems", "UserID", "dbo.Users");
            DropForeignKey("dbo.OrderItems", "Order_OrderID", "dbo.Orders");

            // Drop indexes on these columns
            DropIndex("dbo.OrderItems", new[] { "UserID" });
            DropIndex("dbo.OrderItems", new[] { "Order_OrderID" });

            // Drop the primary key constraint before dropping or renaming columns involved in it
            DropPrimaryKey("dbo.OrderItems");

            // Drop the old OrderID column (was part of PK)
            DropColumn("dbo.OrderItems", "OrderID");

            // Rename foreign key column to OrderID
            RenameColumn(table: "dbo.OrderItems", name: "Order_OrderID", newName: "OrderID");

            // Add new primary key column for OrderItems table
            AddColumn("dbo.OrderItems", "OrderItemID", c => c.Int(nullable: false, identity: true));

            // Make sure OrderID column is non-nullable
            AlterColumn("dbo.OrderItems", "OrderID", c => c.Int(nullable: false));

            // Set the new primary key to the new OrderItemID column
            AddPrimaryKey("dbo.OrderItems", "OrderItemID");

            // Re-create indexes and foreign keys for renamed/modified columns
            CreateIndex("dbo.OrderItems", "OrderID");
            AddForeignKey("dbo.OrderItems", "OrderID", "dbo.Orders", "OrderID", cascadeDelete: true);

            // Remove columns no longer needed in OrderItems
            DropColumn("dbo.OrderItems", "UserID");
            DropColumn("dbo.OrderItems", "OrderDate");
            DropColumn("dbo.OrderItems", "Status");
            DropColumn("dbo.OrderItems", "TotalAmount");
        }

        public override void Down()
        {
            // Reverse of Up - re-add dropped columns and constraints

            AddColumn("dbo.OrderItems", "TotalAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.OrderItems", "Status", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.OrderItems", "OrderDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.OrderItems", "UserID", c => c.Int(nullable: false));

            DropForeignKey("dbo.OrderItems", "OrderID", "dbo.Orders");
            DropIndex("dbo.OrderItems", new[] { "OrderID" });
            DropPrimaryKey("dbo.OrderItems");

            AlterColumn("dbo.OrderItems", "OrderID", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.OrderItems", "OrderItemID");

            AddPrimaryKey("dbo.OrderItems", "OrderID");

            RenameColumn(table: "dbo.OrderItems", name: "OrderID", newName: "Order_OrderID");
            AddColumn("dbo.OrderItems", "OrderID", c => c.Int(nullable: false, identity: true));

            CreateIndex("dbo.OrderItems", "Order_OrderID");
            CreateIndex("dbo.OrderItems", "UserID");

            AddForeignKey("dbo.OrderItems", "Order_OrderID", "dbo.Orders", "OrderID");
            AddForeignKey("dbo.OrderItems", "UserID", "dbo.Users", "UserID", cascadeDelete: true);
        }
    }
}
