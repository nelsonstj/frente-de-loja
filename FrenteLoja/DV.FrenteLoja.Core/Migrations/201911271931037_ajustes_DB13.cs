namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB13 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ORCAMENTO", "IdTabelaPreco", "dbo.TABELA_PRECO");
            DropForeignKey("dbo.TABELA_PRECO_ITEM", "TabelaPrecoId", "dbo.TABELA_PRECO");
            DropIndex("dbo.ORCAMENTO", new[] { "IdTabelaPreco" });
            DropIndex("dbo.TABELA_PRECO_ITEM", new[] { "TabelaPrecoId" });
            //AddColumn("dbo.ORCAMENTO", "TabelaPreco_Id", c => c.Long());
            AddColumn("dbo.TABELA_PRECO", "IdTabelaPreco", c => c.String());
            //AddColumn("dbo.TABELA_PRECO_ITEM", "TabelaPreco_Id", c => c.Long());
            AlterColumn("dbo.ORCAMENTO", "IdTabelaPreco", c => c.String());
            AlterColumn("dbo.TABELA_PRECO_ITEM", "TabelaPrecoId", c => c.String());
            //CreateIndex("dbo.ORCAMENTO", "TabelaPreco_Id");
            //CreateIndex("dbo.TABELA_PRECO_ITEM", "TabelaPreco_Id");
            //AddForeignKey("dbo.ORCAMENTO", "TabelaPreco_Id", "dbo.TABELA_PRECO", "Id");
            //AddForeignKey("dbo.TABELA_PRECO_ITEM", "TabelaPreco_Id", "dbo.TABELA_PRECO", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.TABELA_PRECO_ITEM", "TabelaPreco_Id", "dbo.TABELA_PRECO");
            //DropForeignKey("dbo.ORCAMENTO", "TabelaPreco_Id", "dbo.TABELA_PRECO");
            //DropIndex("dbo.TABELA_PRECO_ITEM", new[] { "TabelaPreco_Id" });
            //DropIndex("dbo.ORCAMENTO", new[] { "TabelaPreco_Id" });
            AlterColumn("dbo.TABELA_PRECO_ITEM", "TabelaPrecoId", c => c.Long(nullable: false));
            AlterColumn("dbo.ORCAMENTO", "IdTabelaPreco", c => c.Long(nullable: false));
            //DropColumn("dbo.TABELA_PRECO_ITEM", "TabelaPreco_Id");
            DropColumn("dbo.TABELA_PRECO", "IdTabelaPreco");
            //DropColumn("dbo.ORCAMENTO", "TabelaPreco_Id");
            CreateIndex("dbo.TABELA_PRECO_ITEM", "TabelaPrecoId");
            CreateIndex("dbo.ORCAMENTO", "IdTabelaPreco");
            AddForeignKey("dbo.TABELA_PRECO_ITEM", "TabelaPrecoId", "dbo.TABELA_PRECO", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ORCAMENTO", "IdTabelaPreco", "dbo.TABELA_PRECO", "Id", cascadeDelete: true);
        }
    }
}
