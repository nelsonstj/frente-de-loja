namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ORCAMENTO", "TRANSPORTADORA_IdTransportadora", "dbo.TRANSPORTADORA");
            DropIndex("dbo.ORCAMENTO", new[] { "IdTransportadora" });
            DropForeignKey("dbo.ORCAMENTO", "VENDEDOR_Vendedor_Id", "dbo.VENDEDOR");
            DropIndex("dbo.ORCAMENTO", new[] { "VENDEDOR_Vendedor_Id" });
        }

        public override void Down()
        {
            DropForeignKey("dbo.ORCAMENTO", "TRANSPORTADORA_IdTransportadora", "dbo.TRANSPORTADORA");
            DropIndex("dbo.ORCAMENTO", new[] { "IdTransportadora" });
            DropForeignKey("dbo.ORCAMENTO", "VENDEDOR_Vendedor_Id", "dbo.VENDEDOR");
            DropIndex("dbo.ORCAMENTO", new[] { "VENDEDOR_Vendedor_Id" });
        }
    }
}
