namespace CarSalesAndInventoryManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        CarID = c.Int(nullable: false, identity: true),
                        Model = c.String(nullable: false, maxLength: 100),
                        Brand = c.String(nullable: false, maxLength: 100),
                        EngineType = c.String(nullable: false, maxLength: 50),
                        Mileage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TankCapacity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Stock = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CarID);
            
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        Status = c.String(nullable: false, maxLength: 50),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Password = c.String(nullable: false, maxLength: 255),
                        Role = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        Status = c.String(nullable: false, maxLength: 50),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Parts",
                c => new
                    {
                        PartID = c.Int(nullable: false, identity: true),
                        PartName = c.String(nullable: false, maxLength: 100),
                        Type = c.String(nullable: false, maxLength: 50),
                        Origin = c.String(nullable: false, maxLength: 50),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Stock = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PartID);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        PaymentID = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Method = c.String(nullable: false, maxLength: 50),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PaymentID)
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .Index(t => t.OrderID);
            
            CreateTable(
                "dbo.TestDrives",
                c => new
                    {
                        TestDriveID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        CarID = c.Int(nullable: false),
                        DateRequested = c.DateTime(nullable: false),
                        Status = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.TestDriveID)
                .ForeignKey("dbo.Cars", t => t.CarID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.CarID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TestDrives", "UserID", "dbo.Users");
            DropForeignKey("dbo.TestDrives", "CarID", "dbo.Cars");
            DropForeignKey("dbo.Payments", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.Orders", "UserID", "dbo.Users");
            DropForeignKey("dbo.OrderItems", "UserID", "dbo.Users");
            DropIndex("dbo.TestDrives", new[] { "CarID" });
            DropIndex("dbo.TestDrives", new[] { "UserID" });
            DropIndex("dbo.Payments", new[] { "OrderID" });
            DropIndex("dbo.Orders", new[] { "UserID" });
            DropIndex("dbo.OrderItems", new[] { "UserID" });
            DropTable("dbo.TestDrives");
            DropTable("dbo.Payments");
            DropTable("dbo.Parts");
            DropTable("dbo.Orders");
            DropTable("dbo.Users");
            DropTable("dbo.OrderItems");
            DropTable("dbo.Cars");
        }
    }
}
