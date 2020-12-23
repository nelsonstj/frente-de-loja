namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB27 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CONVENIO_CONDICAO_PAGAMENTO", "IdCondicaoPagamento", "dbo.CONDICAO_PAGAMENTO");
            DropIndex("dbo.CONVENIO_CONDICAO_PAGAMENTO", new[] { "IdCondicaoPagamento" });
            //AddColumn("dbo.CONVENIO_CONDICAO_PAGAMENTO", "CondicaoPagamento_Id", c => c.Long());
            AlterColumn("dbo.CONVENIO_CONDICAO_PAGAMENTO", "IdCondicaoPagamento", c => c.String());
            //CreateIndex("dbo.CONVENIO_CONDICAO_PAGAMENTO", "CondicaoPagamento_Id");
            //AddForeignKey("dbo.CONVENIO_CONDICAO_PAGAMENTO", "CondicaoPagamento_Id", "dbo.CONDICAO_PAGAMENTO", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.CONVENIO_CONDICAO_PAGAMENTO", "CondicaoPagamento_Id", "dbo.CONDICAO_PAGAMENTO");
            //DropIndex("dbo.CONVENIO_CONDICAO_PAGAMENTO", new[] { "CondicaoPagamento_Id" });
            AlterColumn("dbo.CONVENIO_CONDICAO_PAGAMENTO", "IdCondicaoPagamento", c => c.Long(nullable: false));
            //DropColumn("dbo.CONVENIO_CONDICAO_PAGAMENTO", "CondicaoPagamento_Id");
            CreateIndex("dbo.CONVENIO_CONDICAO_PAGAMENTO", "IdCondicaoPagamento");
            AddForeignKey("dbo.CONVENIO_CONDICAO_PAGAMENTO", "IdCondicaoPagamento", "dbo.CONDICAO_PAGAMENTO", "Id", cascadeDelete: true);
        }
    }
}
