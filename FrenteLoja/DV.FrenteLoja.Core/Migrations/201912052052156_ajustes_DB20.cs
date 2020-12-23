namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB20 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ORCAMENTO_ITEM_EQUIPE_MONTAGEM", "Funcao", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ORCAMENTO_ITEM_EQUIPE_MONTAGEM", "Funcao", c => c.Int(nullable: false));
        }
    }
}
