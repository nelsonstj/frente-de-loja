namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB50 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LOG_INTEGRACAO", "RequestIntegracaoJson", c => c.String());
            AddColumn("dbo.LOG_INTEGRACAO", "ResponseIntegracaoJson", c => c.String());
            DropColumn("dbo.LOG_INTEGRACAO", "DadosIntegracaoJson");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LOG_INTEGRACAO", "DadosIntegracaoJson", c => c.String());
            DropColumn("dbo.LOG_INTEGRACAO", "ResponseIntegracaoJson");
            DropColumn("dbo.LOG_INTEGRACAO", "RequestIntegracaoJson");
        }
    }
}
