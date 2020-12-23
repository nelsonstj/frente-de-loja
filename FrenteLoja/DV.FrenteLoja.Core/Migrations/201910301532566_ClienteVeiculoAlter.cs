namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClienteVeiculoAlter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CLIENTE_VEICULO", "Ano", c => c.Int(nullable: false));
            DropColumn("dbo.CLIENTE_VEICULO", "VeiculoAno");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CLIENTE_VEICULO", "VeiculoAno", c => c.Int(nullable: false));
            DropColumn("dbo.CLIENTE_VEICULO", "Ano");
        }
    }
}
