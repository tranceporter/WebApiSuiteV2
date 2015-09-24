namespace WebAPISuite.Migrations.ClientDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientAutoReply : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientSettings", "AutoReplyToCustomer", c => c.Boolean(nullable: false));
            DropColumn("dbo.ClientSettings", "SendContactUsConfirmation");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ClientSettings", "SendContactUsConfirmation", c => c.Boolean(nullable: false));
            DropColumn("dbo.ClientSettings", "AutoReplyToCustomer");
        }
    }
}
