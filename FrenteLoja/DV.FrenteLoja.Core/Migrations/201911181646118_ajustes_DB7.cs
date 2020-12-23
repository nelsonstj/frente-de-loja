namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB7 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", "TipoVendaId", "dbo.TIPO_VENDA");
            DropIndex("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", new[] { "TipoVendaId" });
            AddColumn("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", "AreaNegocioId", c => c.String());
            //AddColumn("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", "AreaNegocio_IdArea", c => c.String());
            //AddColumn("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", "AreaNegocio_NomeArea", c => c.String());
            DropColumn("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", "TipoVendaId");

            DropForeignKey("dbo.DESCONTO_MODELO_VENDA", "TipoVendaId", "dbo.TIPO_VENDA");
            DropIndex("dbo.DESCONTO_MODELO_VENDA", new[] { "TipoVendaId" });
            AddColumn("dbo.DESCONTO_MODELO_VENDA", "AreaNegocioId", c => c.String());
            //AddColumn("dbo.DESCONTO_MODELO_VENDA", "AreaNegocio_IdArea", c => c.String());
            //AddColumn("dbo.DESCONTO_MODELO_VENDA", "AreaNegocio_NomeArea", c => c.String());
            DropColumn("dbo.DESCONTO_MODELO_VENDA", "TipoVendaId");

            DropForeignKey("dbo.DESCONTO_VENDA_ALCADA", "TipoVendaId", "dbo.TIPO_VENDA");
            DropIndex("dbo.DESCONTO_VENDA_ALCADA", new[] { "TipoVendaId" });
            AddColumn("dbo.DESCONTO_VENDA_ALCADA", "AreaNegocioId", c => c.String());
            //AddColumn("dbo.DESCONTO_VENDA_ALCADA", "AreaNegocio_IdArea", c => c.String());
            //AddColumn("dbo.DESCONTO_VENDA_ALCADA", "AreaNegocio_NomeArea", c => c.String());
            DropColumn("dbo.DESCONTO_VENDA_ALCADA", "TipoVendaId");

            DropForeignKey("dbo.ORCAMENTO", "IdTipoVenda", "dbo.TIPO_VENDA");
            DropForeignKey("dbo.ORCAMENTO", "IdTransportadora", "dbo.TRANSPORTADORA");
            DropIndex("dbo.ORCAMENTO", new[] { "IdTransportadora" });
            DropIndex("dbo.ORCAMENTO", new[] { "IdTipoVenda" });
            //AddColumn("dbo.ORCAMENTO", "TipoVenda_Id", c => c.Long());
            //AddColumn("dbo.ORCAMENTO", "Transportadora_Id", c => c.Long());
            AlterColumn("dbo.ORCAMENTO", "IdTipoVenda", c => c.String());
            //CreateIndex("dbo.ORCAMENTO", "TipoVenda_Id");
            //CreateIndex("dbo.ORCAMENTO", "Transportadora_Id");
            //AddForeignKey("dbo.ORCAMENTO", "TipoVenda_Id", "dbo.TIPO_VENDA", "Id");
            //AddForeignKey("dbo.ORCAMENTO", "Transportadora_Id", "dbo.TRANSPORTADORA", "Id");
        }

        public override void Down()
        {
            AddColumn("dbo.DESCONTO_VENDA_ALCADA", "TipoVendaId", c => c.Long(nullable: false));
            //DropColumn("dbo.DESCONTO_VENDA_ALCADA", "AreaNegocio_NomeArea");
            //DropColumn("dbo.DESCONTO_VENDA_ALCADA", "AreaNegocio_IdArea");
            DropColumn("dbo.DESCONTO_VENDA_ALCADA", "AreaNegocioId");
            CreateIndex("dbo.DESCONTO_VENDA_ALCADA", "TipoVendaId");
            AddForeignKey("dbo.DESCONTO_VENDA_ALCADA", "TipoVendaId", "dbo.TIPO_VENDA", "Id", cascadeDelete: true);

            AddColumn("dbo.DESCONTO_MODELO_VENDA", "TipoVendaId", c => c.Long(nullable: false));
            //DropColumn("dbo.DESCONTO_MODELO_VENDA", "AreaNegocio_NomeArea");
            //DropColumn("dbo.DESCONTO_MODELO_VENDA", "AreaNegocio_IdArea");
            DropColumn("dbo.DESCONTO_MODELO_VENDA", "AreaNegocioId");
            CreateIndex("dbo.DESCONTO_MODELO_VENDA", "TipoVendaId");
            AddForeignKey("dbo.DESCONTO_MODELO_VENDA", "TipoVendaId", "dbo.TIPO_VENDA", "Id", cascadeDelete: true);

            AddColumn("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", "TipoVendaId", c => c.Long(nullable: false));
            //DropColumn("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", "AreaNegocio_NomeArea");
            //DropColumn("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", "AreaNegocio_IdArea");
            DropColumn("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", "AreaNegocioId");
            CreateIndex("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", "TipoVendaId");
            AddForeignKey("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", "TipoVendaId", "dbo.TIPO_VENDA", "Id", cascadeDelete: true);

            //DropForeignKey("dbo.ORCAMENTO", "Transportadora_Id", "dbo.TRANSPORTADORA");
            //DropForeignKey("dbo.ORCAMENTO", "TipoVenda_Id", "dbo.TIPO_VENDA");
            //DropIndex("dbo.ORCAMENTO", new[] { "Transportadora_Id" });
            //DropIndex("dbo.ORCAMENTO", new[] { "TipoVenda_Id" });
            AlterColumn("dbo.ORCAMENTO", "IdTipoVenda", c => c.Long(nullable: false));
            //DropColumn("dbo.ORCAMENTO", "Transportadora_Id");
            //DropColumn("dbo.ORCAMENTO", "TipoVenda_Id");
            CreateIndex("dbo.ORCAMENTO", "IdTipoVenda");
            CreateIndex("dbo.ORCAMENTO", "IdTransportadora");
            AddForeignKey("dbo.ORCAMENTO", "IdTransportadora", "dbo.TRANSPORTADORA", "Id");
            AddForeignKey("dbo.ORCAMENTO", "IdTipoVenda", "dbo.TIPO_VENDA", "Id", cascadeDelete: true);
        }
    }
}
