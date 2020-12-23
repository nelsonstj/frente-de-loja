namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ORCAMENTO", "MARCA_MODELO_VERSAO_MarcaModeloVersao_Id", "dbo.MARCA_MODELO_VERSAO");
            DropForeignKey("dbo.ORCAMENTO", "TRANSPORTADORA_IdTransportadora", "dbo.TRANSPORTADORA");
            DropForeignKey("dbo.ORCAMENTO", "VENDEDOR_Vendedor_Id", "dbo.VENDEDOR");
            DropIndex("dbo.ORCAMENTO", new[] { "MarcaModeloVersao_Id" });
            DropIndex("dbo.ORCAMENTO", new[] { "Operador_Id" });
            DropIndex("dbo.ORCAMENTO", new[] { "Vendedor_Id" });
            DropColumn("dbo.ORCAMENTO", "Operador_Id");
            //DropColumn("dbo.ORCAMENTO", "Vendedor_Id");
        }

        public override void Down()
        {
        }
    }
}
