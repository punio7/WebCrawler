namespace ChatApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaWorkerConnectionIdNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProcessSessions", "WorkerConnectionId", "dbo.WorkerConnections");
            DropIndex("dbo.ProcessSessions", new[] { "WorkerConnectionId" });
            AlterColumn("dbo.ProcessSessions", "WorkerConnectionId", c => c.Long());
            CreateIndex("dbo.ProcessSessions", "WorkerConnectionId");
            AddForeignKey("dbo.ProcessSessions", "WorkerConnectionId", "dbo.WorkerConnections", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProcessSessions", "WorkerConnectionId", "dbo.WorkerConnections");
            DropIndex("dbo.ProcessSessions", new[] { "WorkerConnectionId" });
            AlterColumn("dbo.ProcessSessions", "WorkerConnectionId", c => c.Long(nullable: false));
            CreateIndex("dbo.ProcessSessions", "WorkerConnectionId");
            AddForeignKey("dbo.ProcessSessions", "WorkerConnectionId", "dbo.WorkerConnections", "Id", cascadeDelete: true);
        }
    }
}
