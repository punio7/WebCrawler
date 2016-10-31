namespace ChatApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanieWorkerConnectionState : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkerConnections", "State", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkerConnections", "State");
        }
    }
}
