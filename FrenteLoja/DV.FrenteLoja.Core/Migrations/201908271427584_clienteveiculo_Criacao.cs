namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class clienteveiculo_Criacao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ORCAMENTO", "VeiculoId", c => c.Long(nullable: true));
            CreateIndex("dbo.ORCAMENTO", "VeiculoId");
            AddForeignKey("dbo.ORCAMENTO", "VeiculoId", "dbo.VEICULO", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
        }
    }
}
