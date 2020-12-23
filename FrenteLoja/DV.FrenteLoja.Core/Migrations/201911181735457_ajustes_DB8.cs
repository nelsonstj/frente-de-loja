namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ORCAMENTO", "IdAreaNegocio", c => c.String());
            //AddColumn("dbo.ORCAMENTO", "AreaNegocio_IdArea", c => c.String());
            //AddColumn("dbo.ORCAMENTO", "AreaNegocio_NomeArea", c => c.String());
            DropColumn("dbo.ORCAMENTO", "IdTipoVenda");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ORCAMENTO", "IdTipoVenda", c => c.String());
            //DropColumn("dbo.ORCAMENTO", "AreaNegocio_NomeArea");
            //DropColumn("dbo.ORCAMENTO", "AreaNegocio_IdArea");
            DropColumn("dbo.ORCAMENTO", "IdAreaNegocio");
        }
    }
}
