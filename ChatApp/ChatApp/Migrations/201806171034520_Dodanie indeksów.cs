namespace ChatApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Dodanieindeksów : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ProcessSessions", new[] { "WorkerConnectionId" });
            AlterColumn("dbo.WorkerConnections", "WorkerName", c => c.String(maxLength: 50, unicode: false));
            CreateIndex("dbo.ProcessSessions", "WorkerConnectionId");
            CreateIndex("dbo.ProcessSessions", "State");
            CreateIndex("dbo.WorkerConnections", "WorkerName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.WorkerConnections", new[] { "WorkerName" });
            DropIndex("dbo.ProcessSessions", new[] { "State" });
            DropIndex("dbo.ProcessSessions", new[] { "WorkerConnectionId" });
            AlterColumn("dbo.WorkerConnections", "WorkerName", c => c.String());
            CreateIndex("dbo.ProcessSessions", "WorkerConnectionId");
        }
    }
}
