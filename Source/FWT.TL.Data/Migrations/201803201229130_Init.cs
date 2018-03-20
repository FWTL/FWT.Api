namespace Auth.FWT.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TelegramSession",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Session = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TelegramSession");
        }
    }
}
