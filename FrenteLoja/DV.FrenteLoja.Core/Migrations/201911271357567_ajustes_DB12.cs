namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB12 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TABELA_PRECO_ITEM", "ProdutoId", "dbo.PRODUTO");
            //DropForeignKey("dbo.ORCAMENTO_ITEM", "ProdutoId", "dbo.PRODUTO");
            DropIndex("dbo.TABELA_PRECO_ITEM", new[] { "ProdutoId" });
            //DropIndex("dbo.ORCAMENTO_ITEM", new[] { "ProdutoId" });
            AlterColumn("dbo.TABELA_PRECO_ITEM", "ProdutoId", c => c.String());
            //AlterColumn("dbo.ORCAMENTO_ITEM", "ProdutoId", c => c.String());
            AlterColumn("dbo.GRUPO_SERVICO_AGREGADO_PRODUTO", "IdProduto", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GRUPO_SERVICO_AGREGADO_PRODUTO", "IdProduto", c => c.Long(nullable: false));
            //AlterColumn("dbo.ORCAMENTO_ITEM", "ProdutoId", c => c.Long(nullable: false));
            AlterColumn("dbo.TABELA_PRECO_ITEM", "ProdutoId", c => c.Long(nullable: false));
            //CreateIndex("dbo.ORCAMENTO_ITEM", "ProdutoId");
            CreateIndex("dbo.TABELA_PRECO_ITEM", "ProdutoId");
            //AddForeignKey("dbo.ORCAMENTO_ITEM", "ProdutoId", "dbo.PRODUTO", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TABELA_PRECO_ITEM", "ProdutoId", "dbo.PRODUTO", "Id", cascadeDelete: true);
        }
    }
}
