namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nomequalquer : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CLIENTE", "TipoCliente", c => c.String(maxLength: 1));
            DropColumn("dbo.CONDICAO_PAGAMENTO", "TipoCondicaoPagamento");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CONDICAO_PAGAMENTO", "TipoCondicaoPagamento", c => c.Int(nullable: false));
            AlterColumn("dbo.CLIENTE", "TipoCliente", c => c.String());
        }
    }
}
