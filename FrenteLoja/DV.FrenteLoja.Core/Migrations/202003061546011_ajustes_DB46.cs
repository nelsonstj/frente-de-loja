namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB46 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ORCAMENTO", "LojaDellaVia_Id");

            AddColumn("dbo.ORCAMENTO", "UsuarioCriacao", c => c.String());
        }
        
        public override void Down()
        {
            AddColumn("dbo.ORCAMENTO", "LojaDellaVia_Id", c => c.Long());

            DropColumn("dbo.ORCAMENTO", "UsuarioCriacao");
        }
    }
}
