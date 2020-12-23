namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB14 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ORCAMENTO", "IdConvenio", "dbo.CONVENIO");
            DropIndex("dbo.ORCAMENTO", new[] { "IdConvenio" });
            AddColumn("dbo.CONVENIO", "IdConvenio", c => c.String());
            AlterColumn("dbo.ORCAMENTO", "IdConvenio", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ORCAMENTO", "IdConvenio", c => c.Long(nullable: false));
            DropColumn("dbo.CONVENIO", "IdConvenio");
            CreateIndex("dbo.ORCAMENTO", "IdConvenio");
            AddForeignKey("dbo.ORCAMENTO", "IdConvenio", "dbo.CONVENIO", "Id");
        }
    }
}
