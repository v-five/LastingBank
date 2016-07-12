namespace Lasting.Bank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "Client_ID", c => c.Int());
            CreateIndex("dbo.Accounts", "Client_ID");
            AddForeignKey("dbo.Accounts", "Client_ID", "dbo.Clients", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accounts", "Client_ID", "dbo.Clients");
            DropIndex("dbo.Accounts", new[] { "Client_ID" });
            DropColumn("dbo.Accounts", "Client_ID");
        }
    }
}
