namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTipoCondicaoPagamento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CONDICAO_PAGAMENTO", "TipoCondicaoPagamento", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CONDICAO_PAGAMENTO", "TipoCondicaoPagamento");
        }
    }
}
