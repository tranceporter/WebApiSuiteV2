namespace WebAPISuite.Migrations.ClientDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientInit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        AddressLine1 = c.String(),
                        AddressLine2 = c.String(),
                        Town = c.String(),
                        County = c.String(),
                        Country = c.String(),
                        Phone = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClientSettings",
                c => new
                    {
                        ClientId = c.Guid(nullable: false),
                        SubjectLine = c.String(nullable: false),
                        AutoReplyToCustomer = c.Boolean(nullable: false),
                        EnableFileUpload = c.Boolean(nullable: false),
                        GoogleAdwordsEnabled = c.Boolean(nullable: false),
                        GoogleConversionId = c.Long(nullable: false),
                        GoogleConversionLanguage = c.String(),
                        GoogleConversionFormat = c.String(),
                        GoogleConversionColour = c.String(),
                        GoogleConversionLabel = c.String(),
                        GoogleConversionValue = c.String(),
                        GoogleConversionCurrency = c.String(),
                        GoogleRemarketingOnly = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ClientId)
                .ForeignKey("dbo.Clients", t => t.ClientId)
                .Index(t => t.ClientId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClientSettings", "ClientId", "dbo.Clients");
            DropIndex("dbo.ClientSettings", new[] { "ClientId" });
            DropTable("dbo.ClientSettings");
            DropTable("dbo.Clients");
        }
    }
}
