namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB17 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CONVENIO_CLIENTE", "IdCliente", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CONVENIO_CLIENTE", "IdCliente", c => c.Long(nullable: false));
        }
    }
}
