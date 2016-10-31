namespace ChatApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWorkerAndSession : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProcessSessions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        State = c.Int(nullable: false),
                        CreatorId = c.String(maxLength: 128),
                        AppName = c.String(),
                        WorkerConnection_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorId)
                .ForeignKey("dbo.WorkerConnections", t => t.WorkerConnection_Id)
                .Index(t => t.CreatorId)
                .Index(t => t.WorkerConnection_Id);
            
            CreateTable(
                "dbo.WorkerConnections",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        WorkerName = c.String(),
                        ConnectionId = c.String(),
                        ActiveSessions = c.Int(nullable: false),
                        MaxSessions = c.Int(nullable: false),
                        AllSessions = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProcessSessions", "WorkerConnection_Id", "dbo.WorkerConnections");
            DropForeignKey("dbo.ProcessSessions", "CreatorId", "dbo.AspNetUsers");
            DropIndex("dbo.ProcessSessions", new[] { "WorkerConnection_Id" });
            DropIndex("dbo.ProcessSessions", new[] { "CreatorId" });
            DropTable("dbo.WorkerConnections");
            DropTable("dbo.ProcessSessions");
        }
    }
}
