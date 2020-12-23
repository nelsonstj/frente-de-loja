namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB23 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ORCAMENTO", "Ano", c => c.Int());
            AlterColumn("dbo.ORCAMENTO", "KM", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ORCAMENTO", "KM", c => c.Int(nullable: false));
            AlterColumn("dbo.ORCAMENTO", "Ano", c => c.Int(nullable: false));
        }
    }
}
