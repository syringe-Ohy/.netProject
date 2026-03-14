namespace CarSalesAndInventoryManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyMyTable4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CarRequests",
                c => new
                    {
                        CarRequestID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        Brand = c.String(nullable: false, maxLength: 100),
                        Model = c.String(nullable: false, maxLength: 100),
                        EngineType = c.String(maxLength: 50),
                        RequestDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Status = c.String(nullable: false, maxLength: 50),
                        Notes = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.CarRequestID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CarRequests", "UserID", "dbo.Users");
            DropIndex("dbo.CarRequests", new[] { "UserID" });
            DropTable("dbo.CarRequests");
        }
    }
}
