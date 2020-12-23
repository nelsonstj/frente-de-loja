namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB9 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "CondicaoPagamento_Id");
            DropForeignKey("dbo.ORCAMENTO_ITEM", "ProdutoId", "dbo.PRODUTO");
            DropIndex("dbo.ORCAMENTO_ITEM", new[] { "ProdutoId" });
            AlterColumn("dbo.ORCAMENTO_ITEM", "ProdutoId", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "CondicaoPagamento_Id");
            DropForeignKey("dbo.ORCAMENTO_ITEM", "ProdutoId", "dbo.PRODUTO");
            DropIndex("dbo.ORCAMENTO_ITEM", new[] { "ProdutoId" });
            AlterColumn("dbo.ORCAMENTO_ITEM", "ProdutoId", c => c.Long(nullable: false));
        }
    }
}
