namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB48 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ORCAMENTO", "LojaDellaVia_Id", c => c.Long());
        }

        public override void Down()
        {
            DropColumn("dbo.ORCAMENTO", "LojaDellaVia_Id");
        }
    }
}
