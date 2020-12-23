namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB6 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ORCAMENTO", "MarcaModeloVersao_Id", "dbo.MARCA_MODELO_VERSAO");
            DropForeignKey("dbo.ORCAMENTO", "IdTransportadora", "dbo.TRANSPORTADORA");
            DropForeignKey("dbo.ORCAMENTO", "Vendedor_Id", "dbo.VENDEDOR");
            DropColumn("dbo.ORCAMENTO", "Vendedor_Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.ORCAMENTO", "_MarcaModeloVersao_Id", "dbo.MARCA_MODELO_VERSAO");
            DropForeignKey("dbo.ORCAMENTO", "IdTransportadora", "dbo.TRANSPORTADORA");
            DropForeignKey("dbo.ORCAMENTO", "Vendedor_Id", "dbo.VENDEDOR");
            DropColumn("dbo.ORCAMENTO", "Vendedor_Id");
        }
    }
}
