namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB45 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LOG_INTEGRACAO", "DadosIntegracaoJson", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LOG_INTEGRACAO", "DadosIntegracaoJson");
        }
    }
}
