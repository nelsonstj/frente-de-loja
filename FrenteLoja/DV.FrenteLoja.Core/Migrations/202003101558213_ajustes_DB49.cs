namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB49 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LOG_INTEGRACAO", "Log", c => c.String());
            DropColumn("dbo.LOG_INTEGRACAO", "LogErro");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LOG_INTEGRACAO", "LogErro", c => c.String());
            DropColumn("dbo.LOG_INTEGRACAO", "Log");
        }
    }
}
