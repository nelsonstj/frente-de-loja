namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB44 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", "LojaDellaviaId", "dbo.LOJA_DELLAVIA");
            DropForeignKey("dbo.DESCONTO_VENDA_ALCADA", "LojaDellaviaId", "dbo.LOJA_DELLAVIA");
            DropIndex("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", new[] { "LojaDellaviaId" });
            DropIndex("dbo.DESCONTO_VENDA_ALCADA", new[] { "LojaDellaviaId" });
            //AddColumn("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", "LojaDellaVia_Id", c => c.Long());
            //AddColumn("dbo.DESCONTO_VENDA_ALCADA", "LojaDellaVia_Id", c => c.Long());
            AlterColumn("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", "LojaDellaviaId", c => c.String());
            AlterColumn("dbo.DESCONTO_VENDA_ALCADA", "LojaDellaviaId", c => c.String());
            //CreateIndex("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", "LojaDellaVia_Id");
            //CreateIndex("dbo.DESCONTO_VENDA_ALCADA", "LojaDellaVia_Id");
            //AddForeignKey("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", "LojaDellaVia_Id", "dbo.LOJA_DELLAVIA", "Id");
            //AddForeignKey("dbo.DESCONTO_VENDA_ALCADA", "LojaDellaVia_Id", "dbo.LOJA_DELLAVIA", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.DESCONTO_VENDA_ALCADA", "LojaDellaVia_Id", "dbo.LOJA_DELLAVIA");
            //DropForeignKey("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", "LojaDellaVia_Id", "dbo.LOJA_DELLAVIA");
            //DropIndex("dbo.DESCONTO_VENDA_ALCADA", new[] { "LojaDellaVia_Id" });
            //DropIndex("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", new[] { "LojaDellaVia_Id" });
            AlterColumn("dbo.DESCONTO_VENDA_ALCADA", "LojaDellaviaId", c => c.Long(nullable: false));
            AlterColumn("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", "LojaDellaviaId", c => c.Long(nullable: false));
            //DropColumn("dbo.DESCONTO_VENDA_ALCADA", "LojaDellaVia_Id");
            //DropColumn("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", "LojaDellaVia_Id");
            CreateIndex("dbo.DESCONTO_VENDA_ALCADA", "LojaDellaviaId");
            CreateIndex("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", "LojaDellaviaId");
            AddForeignKey("dbo.DESCONTO_VENDA_ALCADA", "LojaDellaviaId", "dbo.LOJA_DELLAVIA", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DESCONTO_VENDA_ALCADA_GRUPO_PRODUTO", "LojaDellaviaId", "dbo.LOJA_DELLAVIA", "Id", cascadeDelete: true);
        }
    }
}
