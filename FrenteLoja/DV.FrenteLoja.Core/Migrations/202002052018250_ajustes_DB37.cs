namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB37 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ORCAMENTO", "IdLojaDellaVia", "dbo.LOJA_DELLAVIA");
            DropIndex("dbo.ORCAMENTO", new[] { "IdLojaDellaVia" });
            //AddColumn("dbo.ORCAMENTO", "LojaDellaVia_Id", c => c.Long());
            AlterColumn("dbo.ORCAMENTO", "IdLojaDellaVia", c => c.String());
            //CreateIndex("dbo.ORCAMENTO", "LojaDellaVia_Id");
            //AddForeignKey("dbo.ORCAMENTO", "LojaDellaVia_Id", "dbo.LOJA_DELLAVIA", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.ORCAMENTO", "LojaDellaVia_Id", "dbo.LOJA_DELLAVIA");
            //DropIndex("dbo.ORCAMENTO", new[] { "LojaDellaVia_Id" });
            AlterColumn("dbo.ORCAMENTO", "IdLojaDellaVia", c => c.Long(nullable: false));
            //DropColumn("dbo.ORCAMENTO", "LojaDellaVia_Id");
            CreateIndex("dbo.ORCAMENTO", "IdLojaDellaVia");
            AddForeignKey("dbo.ORCAMENTO", "IdLojaDellaVia", "dbo.LOJA_DELLAVIA", "Id", cascadeDelete: true);
        }
    }
}
