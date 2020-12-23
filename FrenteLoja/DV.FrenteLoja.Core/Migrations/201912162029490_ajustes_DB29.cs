namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB29 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CONVENIO_CLIENTE", new[] { "CampoCodigo" });
            //AddColumn("dbo.CONVENIO_CLIENTE", "Descricao", c => c.String());
            AlterColumn("dbo.CONVENIO_CLIENTE", "CampoCodigo", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CONVENIO_CLIENTE", "CampoCodigo", c => c.String(maxLength: 12));
            //DropColumn("dbo.CONVENIO_CLIENTE", "Descricao");
            CreateIndex("dbo.CONVENIO_CLIENTE", "CampoCodigo", unique: true);
        }
    }
}
