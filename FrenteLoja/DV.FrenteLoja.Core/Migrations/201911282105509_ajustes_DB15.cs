namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB15 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CONVENIO_CLIENTE", "IdConvenio", "dbo.CONVENIO");
            DropForeignKey("dbo.CONVENIO_CONDICAO_PAGAMENTO", "IdConvenio", "dbo.CONVENIO");
            DropForeignKey("dbo.CONVENIO_PRODUTO", "IdConvenio", "dbo.CONVENIO");
            DropIndex("dbo.CONVENIO_CLIENTE", new[] { "IdConvenio" });
            DropIndex("dbo.CONVENIO_CONDICAO_PAGAMENTO", new[] { "IdConvenio" });
            DropIndex("dbo.CONVENIO_PRODUTO", new[] { "IdConvenio" });
            //AddColumn("dbo.CONVENIO_CLIENTE", "Convenio_Id", c => c.Long());
            //AddColumn("dbo.CONVENIO_CONDICAO_PAGAMENTO", "Convenio_Id", c => c.Long());
            //AddColumn("dbo.CONVENIO_PRODUTO", "Convenio_Id", c => c.Long());
            AlterColumn("dbo.CONVENIO_CLIENTE", "IdConvenio", c => c.String());
            AlterColumn("dbo.CONVENIO_CONDICAO_PAGAMENTO", "IdConvenio", c => c.String());
            AlterColumn("dbo.CONVENIO_PRODUTO", "IdConvenio", c => c.String());
            //CreateIndex("dbo.CONVENIO_CLIENTE", "Convenio_Id");
            //CreateIndex("dbo.CONVENIO_CONDICAO_PAGAMENTO", "Convenio_Id");
            //CreateIndex("dbo.CONVENIO_PRODUTO", "Convenio_Id");
            //AddForeignKey("dbo.CONVENIO_CLIENTE", "Convenio_Id", "dbo.CONVENIO", "Id");
            //AddForeignKey("dbo.CONVENIO_CONDICAO_PAGAMENTO", "Convenio_Id", "dbo.CONVENIO", "Id");
            //AddForeignKey("dbo.CONVENIO_PRODUTO", "Convenio_Id", "dbo.CONVENIO", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.CONVENIO_PRODUTO", "Convenio_Id", "dbo.CONVENIO");
            //DropForeignKey("dbo.CONVENIO_CONDICAO_PAGAMENTO", "Convenio_Id", "dbo.CONVENIO");
            //DropForeignKey("dbo.CONVENIO_CLIENTE", "Convenio_Id", "dbo.CONVENIO");
            //DropIndex("dbo.CONVENIO_PRODUTO", new[] { "Convenio_Id" });
            //DropIndex("dbo.CONVENIO_CONDICAO_PAGAMENTO", new[] { "Convenio_Id" });
            //DropIndex("dbo.CONVENIO_CLIENTE", new[] { "Convenio_Id" });
            AlterColumn("dbo.CONVENIO_PRODUTO", "IdConvenio", c => c.Long(nullable: false));
            AlterColumn("dbo.CONVENIO_CONDICAO_PAGAMENTO", "IdConvenio", c => c.Long(nullable: false));
            AlterColumn("dbo.CONVENIO_CLIENTE", "IdConvenio", c => c.Long(nullable: false));
            //DropColumn("dbo.CONVENIO_PRODUTO", "Convenio_Id");
            //DropColumn("dbo.CONVENIO_CONDICAO_PAGAMENTO", "Convenio_Id");
            //DropColumn("dbo.CONVENIO_CLIENTE", "Convenio_Id");
            CreateIndex("dbo.CONVENIO_PRODUTO", "IdConvenio");
            CreateIndex("dbo.CONVENIO_CONDICAO_PAGAMENTO", "IdConvenio");
            CreateIndex("dbo.CONVENIO_CLIENTE", "IdConvenio");
            AddForeignKey("dbo.CONVENIO_PRODUTO", "IdConvenio", "dbo.CONVENIO", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CONVENIO_CONDICAO_PAGAMENTO", "IdConvenio", "dbo.CONVENIO", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CONVENIO_CLIENTE", "IdConvenio", "dbo.CONVENIO", "Id", cascadeDelete: true);
        }
    }
}
