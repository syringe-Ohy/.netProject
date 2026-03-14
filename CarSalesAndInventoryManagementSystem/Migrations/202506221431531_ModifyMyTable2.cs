namespace CarSalesAndInventoryManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyMyTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Email", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.Users", "DOB", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "DOB");
            DropColumn("dbo.Users", "Email");
        }
    }
}
