namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB36 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "PercentualAcrescimo", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "PercentualAcrescimo");
        }
    }
}
