namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB34 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ORCAMENTO_ITEM", "PercDescon", c => c.Decimal(nullable: false, precision: 18, scale: 4));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ORCAMENTO_ITEM", "PercDescon", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
