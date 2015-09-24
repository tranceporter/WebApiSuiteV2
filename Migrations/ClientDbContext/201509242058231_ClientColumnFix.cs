namespace WebAPISuite.Migrations.ClientDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientColumnFix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientSettings", "GoogleConversionId", c => c.Long(nullable: false));
            DropColumn("dbo.ClientSettings", "GoogleConverionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ClientSettings", "GoogleConverionId", c => c.Long(nullable: false));
            DropColumn("dbo.ClientSettings", "GoogleConversionId");
        }
    }
}
