namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ORCAMENTO_FORMA_PAGAMENTO", "IdCondicaoPagamento", "dbo.CONDICAO_PAGAMENTO");
            DropIndex("dbo.ORCAMENTO_FORMA_PAGAMENTO", new[] { "IdCondicaoPagamento" });
            DropForeignKey("dbo.ORCAMENTO_FORMA_PAGAMENTO", "CondicaoPagamento_Id", "dbo.CONDICAO_PAGAMENTO");
            DropIndex("dbo.ORCAMENTO_FORMA_PAGAMENTO", new[] { "CondicaoPagamento_Id" });
        }

        public override void Down()
        {
            DropForeignKey("dbo.ORCAMENTO_FORMA_PAGAMENTO", "IdCondicaoPagamento", "dbo.CONDICAO_PAGAMENTO");
            DropIndex("dbo.ORCAMENTO_FORMA_PAGAMENTO", new[] { "IdCondicaoPagamento" });
            DropForeignKey("dbo.ORCAMENTO_FORMA_PAGAMENTO", "CondicaoPagamento_Id", "dbo.CONDICAO_PAGAMENTO");
            DropIndex("dbo.ORCAMENTO_FORMA_PAGAMENTO", new[] { "CondicaoPagamento_Id" });
        }
    }
}
