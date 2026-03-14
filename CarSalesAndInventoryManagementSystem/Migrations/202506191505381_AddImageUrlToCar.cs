namespace CarSalesAndInventoryManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImageUrlToCar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cars", "ImageUrl", c => c.String(maxLength: 1024));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cars", "ImageUrl");
        }
    }
}
