namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB26 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TABELA_PRECO", "DataAte", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TABELA_PRECO", "DataAte", c => c.DateTime(nullable: false));
        }
    }
}
