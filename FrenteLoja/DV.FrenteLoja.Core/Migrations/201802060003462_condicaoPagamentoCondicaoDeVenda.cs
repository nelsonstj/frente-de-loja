namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class condicaoPagamentoCondicaoDeVenda : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CONDICAO_PAGAMENTO", "CondicaoDeVenda", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CONDICAO_PAGAMENTO", "CondicaoDeVenda");
        }
    }
}
