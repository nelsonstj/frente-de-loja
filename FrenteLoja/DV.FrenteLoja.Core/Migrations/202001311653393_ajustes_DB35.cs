namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB35 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "CondicaoPagamento_ValorAcrescimo", c => c.Decimal(nullable: true, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "CondicaoPagamento_ValorAcrescimo");
        }
    }
}
