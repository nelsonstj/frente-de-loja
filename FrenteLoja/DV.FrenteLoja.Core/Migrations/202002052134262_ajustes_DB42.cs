namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB42 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ORCAMENTO", "LojaDellaVia_Id", "dbo.LOJA_DELLAVIA");
            DropIndex("dbo.ORCAMENTO", new[] { "LojaDellaVia_Id" });
            DropColumn("dbo.ORCAMENTO", "LojaDellaVia_Id");

            //AddColumn("dbo.ORCAMENTO", "LojaDellaVia_Id", c => c.Long());
            //CreateIndex("dbo.ORCAMENTO", "LojaDellaVia_Id");
            //AddForeignKey("dbo.ORCAMENTO", "LojaDellaVia_Id", "dbo.LOJA_DELLAVIA", "Id");
        }

        public override void Down()
        {
            AddColumn("dbo.ORCAMENTO", "LojaDellaVia_Id", c => c.Long());
            CreateIndex("dbo.ORCAMENTO", "LojaDellaVia_Id");
            AddForeignKey("dbo.ORCAMENTO", "LojaDellaVia_Id", "dbo.LOJA_DELLAVIA", "Id");

            //DropForeignKey("dbo.ORCAMENTO", "LojaDellaVia_Id", "dbo.LOJA_DELLAVIA");
            //DropIndex("dbo.ORCAMENTO", new[] { "LojaDellaVia_Id" });
            //DropColumn("dbo.ORCAMENTO", "LojaDellaVia_Id");
        }
    }
}
