namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB25 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CONVENIO", "DataFimVigencia", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CONVENIO", "DataFimVigencia", c => c.DateTime(nullable: false));
        }
    }
}
