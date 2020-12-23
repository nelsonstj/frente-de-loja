namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ORCAMENTO_FORMA_PAGAMENTO", "IdCondicaoPagamento", "dbo.CONDICAO_PAGAMENTO");
            DropIndex("dbo.ORCAMENTO_FORMA_PAGAMENTO", new[] { "IdCondicaoPagamento" });
            AddColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "CondicaoPagamento_Id", c => c.Long());
            AlterColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "IdCondicaoPagamento", c => c.String());
            CreateIndex("dbo.ORCAMENTO_FORMA_PAGAMENTO", "CondicaoPagamento_Id");
            AddForeignKey("dbo.ORCAMENTO_FORMA_PAGAMENTO", "CondicaoPagamento_Id", "dbo.CONDICAO_PAGAMENTO", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ORCAMENTO_FORMA_PAGAMENTO", "CondicaoPagamento_Id", "dbo.CONDICAO_PAGAMENTO");
            DropIndex("dbo.ORCAMENTO_FORMA_PAGAMENTO", new[] { "CondicaoPagamento_Id" });
            AlterColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "IdCondicaoPagamento", c => c.Long(nullable: false));
            DropColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "CondicaoPagamento_Id");
            CreateIndex("dbo.ORCAMENTO_FORMA_PAGAMENTO", "IdCondicaoPagamento");
            AddForeignKey("dbo.ORCAMENTO_FORMA_PAGAMENTO", "IdCondicaoPagamento", "dbo.CONDICAO_PAGAMENTO", "Id", cascadeDelete: true);
        }
    }
}
