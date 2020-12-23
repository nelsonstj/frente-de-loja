namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixModeloVenda : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DESCONTO_MODELO_VENDA", "CampoCodigo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DESCONTO_MODELO_VENDA", "CampoCodigo");
        }
    }
}
