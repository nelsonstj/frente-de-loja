namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveUKCampoCodidoMarcaEModelo : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.MARCA_MODELO", new[] { "CampoCodigo" });
            DropIndex("dbo.MARCA", new[] { "CampoCodigo" });
            CreateIndex("dbo.MARCA_MODELO", "CampoCodigo");
            CreateIndex("dbo.MARCA", "CampoCodigo");
        }
        
        public override void Down()
        {
            DropIndex("dbo.MARCA", new[] { "CampoCodigo" });
            DropIndex("dbo.MARCA_MODELO", new[] { "CampoCodigo" });
            CreateIndex("dbo.MARCA", "CampoCodigo", unique: true);
            CreateIndex("dbo.MARCA_MODELO", "CampoCodigo", unique: true);
        }
    }
}
