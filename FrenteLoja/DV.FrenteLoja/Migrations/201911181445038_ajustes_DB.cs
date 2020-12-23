namespace DV.FrenteLoja.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ORCAMENTO", "IdTipoVenda", "dbo.TIPO_VENDA");
            DropIndex("dbo.ORCAMENTO", new[] { "IdTipoVenda" });
        }

        public override void Down()
        {
            DropForeignKey("dbo.ORCAMENTO", "IdTipoVenda", "dbo.TIPO_VENDA");
            DropIndex("dbo.ORCAMENTO", new[] { "IdTipoVenda" });
        }
    }
}
