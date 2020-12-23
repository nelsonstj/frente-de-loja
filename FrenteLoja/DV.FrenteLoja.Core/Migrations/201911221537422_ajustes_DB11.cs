namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB11 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ORCAMENTO", "MarcaModeloVersao_Id");
        }

        public override void Down()
        {
            DropColumn("dbo.ORCAMENTO", "MarcaModeloVersao_Id");
        }
    }
}
