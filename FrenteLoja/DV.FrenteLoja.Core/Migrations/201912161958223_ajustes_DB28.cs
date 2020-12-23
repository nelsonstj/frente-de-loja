namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB28 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CONVENIO_CLIENTE", "IdLoja", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CONVENIO_CLIENTE", "IdLoja");
        }
    }
}
