namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB22 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ORCAMENTO_ITEM_EQUIPE_MONTAGEM", "IdVendedor", "dbo.VENDEDOR");
            DropIndex("dbo.ORCAMENTO_ITEM_EQUIPE_MONTAGEM", new[] { "IdVendedor" });
            AlterColumn("dbo.ORCAMENTO_ITEM_EQUIPE_MONTAGEM", "IdVendedor", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ORCAMENTO_ITEM_EQUIPE_MONTAGEM", "IdVendedor", c => c.Long(nullable: false));
            CreateIndex("dbo.ORCAMENTO_ITEM_EQUIPE_MONTAGEM", "IdVendedor");
            AddForeignKey("dbo.ORCAMENTO_ITEM_EQUIPE_MONTAGEM", "IdVendedor", "dbo.VENDEDOR", "Id");
        }
    }
}
