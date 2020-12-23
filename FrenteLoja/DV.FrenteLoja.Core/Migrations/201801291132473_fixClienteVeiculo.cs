namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixClienteVeiculo : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CLIENTE_VEICULO", "Placa", c => c.String(maxLength: 10));
            AlterColumn("dbo.CLIENTE_VEICULO", "Observacoes", c => c.String(maxLength: 200));
            AlterColumn("dbo.CLIENTE_VEICULO", "UsuarioAtualizacao", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CLIENTE_VEICULO", "UsuarioAtualizacao", c => c.String());
            AlterColumn("dbo.CLIENTE_VEICULO", "Observacoes", c => c.String());
            AlterColumn("dbo.CLIENTE_VEICULO", "Placa", c => c.String());
        }
    }
}
