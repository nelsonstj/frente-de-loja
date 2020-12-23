namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB18 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ORCAMENTO_FORMA_PAGAMENTO", "IdAdministradoraFinanceira", "dbo.ADMINISTRACAO_FINANCEIRA");
            DropIndex("dbo.ORCAMENTO_FORMA_PAGAMENTO", new[] { "IdAdministradoraFinanceira" });
            //AddColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "AdministradoraFinanceira_Id", c => c.Long());
            AddColumn("dbo.ADMINISTRACAO_FINANCEIRA", "IdAdminFinanceira", c => c.String());
            AlterColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "IdAdministradoraFinanceira", c => c.String());
            //CreateIndex("dbo.ORCAMENTO_FORMA_PAGAMENTO", "AdministradoraFinanceira_Id");
            //AddForeignKey("dbo.ORCAMENTO_FORMA_PAGAMENTO", "AdministradoraFinanceira_Id", "dbo.ADMINISTRACAO_FINANCEIRA", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.ORCAMENTO_FORMA_PAGAMENTO", "AdministradoraFinanceira_Id", "dbo.ADMINISTRACAO_FINANCEIRA");
            //DropIndex("dbo.ORCAMENTO_FORMA_PAGAMENTO", new[] { "AdministradoraFinanceira_Id" });
            AlterColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "IdAdministradoraFinanceira", c => c.Long());
            DropColumn("dbo.ADMINISTRACAO_FINANCEIRA", "IdAdminFinanceira");
            //DropColumn("dbo.ORCAMENTO_FORMA_PAGAMENTO", "AdministradoraFinanceira_Id");
            CreateIndex("dbo.ORCAMENTO_FORMA_PAGAMENTO", "IdAdministradoraFinanceira");
            AddForeignKey("dbo.ORCAMENTO_FORMA_PAGAMENTO", "IdAdministradoraFinanceira", "dbo.ADMINISTRACAO_FINANCEIRA", "Id");
        }
    }
}
