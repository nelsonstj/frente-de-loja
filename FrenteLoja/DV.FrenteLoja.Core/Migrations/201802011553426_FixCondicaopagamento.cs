namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixCondicaopagamento : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CONDICAO_PAGAMENTO", "TipoCondicaoPagamento", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CONDICAO_PAGAMENTO", "TipoCondicaoPagamento", c => c.Int(nullable: false));
        }
    }
}
