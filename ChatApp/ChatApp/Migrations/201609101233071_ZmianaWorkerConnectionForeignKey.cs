namespace ChatApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaWorkerConnectionForeignKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProcessSessions", "WorkerConnection_Id", "dbo.WorkerConnections");
            DropIndex("dbo.ProcessSessions", new[] { "WorkerConnection_Id" });
            RenameColumn(table: "dbo.ProcessSessions", name: "WorkerConnection_Id", newName: "WorkerConnectionId");
            AlterColumn("dbo.ProcessSessions", "WorkerConnectionId", c => c.Long(nullable: false));
            CreateIndex("dbo.ProcessSessions", "WorkerConnectionId");
            AddForeignKey("dbo.ProcessSessions", "WorkerConnectionId", "dbo.WorkerConnections", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProcessSessions", "WorkerConnectionId", "dbo.WorkerConnections");
            DropIndex("dbo.ProcessSessions", new[] { "WorkerConnectionId" });
            AlterColumn("dbo.ProcessSessions", "WorkerConnectionId", c => c.Long());
            RenameColumn(table: "dbo.ProcessSessions", name: "WorkerConnectionId", newName: "WorkerConnection_Id");
            CreateIndex("dbo.ProcessSessions", "WorkerConnection_Id");
            AddForeignKey("dbo.ProcessSessions", "WorkerConnection_Id", "dbo.WorkerConnections", "Id");
        }
    }
}
