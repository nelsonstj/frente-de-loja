namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB3 : DbMigration
    {
        public override void Up()
        {
        }

        public override void Down()
        {
            DropForeignKey("dbo.ORCAMENTO", "TRANSPORTADORA_IdTransportadora", "dbo.TRANSPORTADORA");
            DropForeignKey("dbo.ORCAMENTO", "VENDEDOR_Vendedor_Id", "dbo.VENDEDOR");
            DropForeignKey("dbo.ORCAMENTO", "MARCA_MODELO_VERSAO_MarcaModeloVersao_Id", "dbo.MARCA_MODELO_VERSAO");
            DropIndex("dbo.ORCAMENTO", new[] { "Vendedor_Id" });
            DropIndex("dbo.ORCAMENTO", new[] { "Operador_Id" });
            DropIndex("dbo.ORCAMENTO", new[] { "MarcaModeloVersao_Id" });
            DropColumn("dbo.ORCAMENTO", "Operador_Id");
            DropColumn("dbo.ORCAMENTO", "Vendedor_Id");
        }
    }
}
