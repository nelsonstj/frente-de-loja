namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB21 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ORCAMENTO_ITEM_EQUIPE_MONTAGEM", "Funcao", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ORCAMENTO_ITEM_EQUIPE_MONTAGEM", "Funcao", c => c.String());
        }
    }
}
