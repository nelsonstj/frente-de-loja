namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB10 : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.ORCAMENTO_FORMA_PAGAMENTO", "IdOrcamento", "dbo.ORCAMENTO");
            //DropForeignKey("dbo.ORCAMENTO_FORMA_PAGAMENTO", "IdBanco", "dbo.BANCO");
            //DropIndex("dbo.ORCAMENTO_FORMA_PAGAMENTO", new[] { "IdOrcamento" });
            //DropIndex("dbo.ORCAMENTO_FORMA_PAGAMENTO", new[] { "IdBanco" });
            //AddColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "CondicaoPagamento_IdCondicaoPagamento", c => c.String());
            //AddColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "CondicaoPagamento_NomeCondicaoPagamento", c => c.String());
            //AddColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "CondicaoPagamento_DescricaoCondicaoPagamento", c => c.String());
            //AddColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "CondicaoPagamento_IdFormaPagamento", c => c.String());
            //AddColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "CondicaoPagamento_PrazoMedio", c => c.String());
            //AddColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "CondicaoPagamento_QtdParcelas", c => c.Int(nullable: false));
            //AddColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "Banco_Id", c => c.Long());
            //AddColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "Orcamento_Id", c => c.Long());
            //CreateIndex("dbo.ORCAMENTO_FORMA_PAGAMENTO", "Banco_Id");
            //CreateIndex("dbo.ORCAMENTO_FORMA_PAGAMENTO", "Orcamento_Id");
            //AddForeignKey("dbo.ORCAMENTO_FORMA_PAGAMENTO", "Orcamento_Id", "dbo.ORCAMENTO", "Id");
            //AddForeignKey("dbo.ORCAMENTO_FORMA_PAGAMENTO", "Banco_Id", "dbo.BANCO", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.ORCAMENTO_FORMA_PAGAMENTO", "Banco_Id", "dbo.BANCO");
            //DropForeignKey("dbo.ORCAMENTO_FORMA_PAGAMENTO", "Orcamento_Id", "dbo.ORCAMENTO");
            //DropIndex("dbo.ORCAMENTO_FORMA_PAGAMENTO", new[] { "Orcamento_Id" });
            //DropIndex("dbo.ORCAMENTO_FORMA_PAGAMENTO", new[] { "Banco_Id" });
            //DropColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "Orcamento_Id");
            //DropColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "Banco_Id");
            //DropColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "CondicaoPagamento_QtdParcelas");
            //DropColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "CondicaoPagamento_PrazoMedio");
            //DropColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "CondicaoPagamento_IdFormaPagamento");
            //DropColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "CondicaoPagamento_DescricaoCondicaoPagamento");
            //DropColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "CondicaoPagamento_NomeCondicaoPagamento");
            //DropColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "CondicaoPagamento_IdCondicaoPagamento");
            //CreateIndex("dbo.ORCAMENTO_FORMA_PAGAMENTO", "IdBanco");
            //CreateIndex("dbo.ORCAMENTO_FORMA_PAGAMENTO", "IdOrcamento");
            //AddForeignKey("dbo.ORCAMENTO_FORMA_PAGAMENTO", "IdBanco", "dbo.BANCO", "Id");
            //AddForeignKey("dbo.ORCAMENTO_FORMA_PAGAMENTO", "IdOrcamento", "dbo.ORCAMENTO", "Id", cascadeDelete: true);
        }
    }
}
