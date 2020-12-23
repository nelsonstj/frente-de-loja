namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB16 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ORCAMENTO", "IdCliente", "dbo.CLIENTE");
            DropForeignKey("dbo.CONVENIO", "IdCliente", "dbo.CLIENTE");
            DropForeignKey("dbo.CONVENIO", "IdTabelaPreco", "dbo.TABELA_PRECO");
            DropForeignKey("dbo.CLIENTE_VEICULO", "ClienteId", "dbo.CLIENTE");
            DropIndex("dbo.ORCAMENTO", new[] { "IdCliente" });
            DropIndex("dbo.CONVENIO", new[] { "IdTabelaPreco" });
            DropIndex("dbo.CONVENIO", new[] { "IdCliente" });
            DropIndex("dbo.CLIENTE_VEICULO", new[] { "ClienteId" });
            //AddColumn("dbo.ORCAMENTO", "Cliente_Id", c => c.Long());
            AddColumn("dbo.CLIENTE", "IdCliente", c => c.String());
            //AddColumn("dbo.CONVENIO", "Cliente_Id", c => c.Long());
            //AddColumn("dbo.CONVENIO", "TabelaPreco_Id", c => c.Long());
            //AddColumn("dbo.CLIENTE_VEICULO", "Cliente_Id", c => c.Long());
            AlterColumn("dbo.ORCAMENTO", "IdCliente", c => c.String());
            AlterColumn("dbo.CONVENIO", "IdTabelaPreco", c => c.String());
            AlterColumn("dbo.CONVENIO", "IdCliente", c => c.String());
            AlterColumn("dbo.CLIENTE_VEICULO", "ClienteId", c => c.String());
            //CreateIndex("dbo.ORCAMENTO", "Cliente_Id");
            //CreateIndex("dbo.CONVENIO", "Cliente_Id");
            //CreateIndex("dbo.CONVENIO", "TabelaPreco_Id");
            //CreateIndex("dbo.CLIENTE_VEICULO", "Cliente_Id");
            //AddForeignKey("dbo.ORCAMENTO", "Cliente_Id", "dbo.CLIENTE", "Id");
            //AddForeignKey("dbo.CONVENIO", "Cliente_Id", "dbo.CLIENTE", "Id");
            //AddForeignKey("dbo.CONVENIO", "TabelaPreco_Id", "dbo.TABELA_PRECO", "Id");
            //AddForeignKey("dbo.CLIENTE_VEICULO", "Cliente_Id", "dbo.CLIENTE", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.CLIENTE_VEICULO", "Cliente_Id", "dbo.CLIENTE");
            //DropForeignKey("dbo.CONVENIO", "TabelaPreco_Id", "dbo.TABELA_PRECO");
            //DropForeignKey("dbo.CONVENIO", "Cliente_Id", "dbo.CLIENTE");
            //DropForeignKey("dbo.ORCAMENTO", "Cliente_Id", "dbo.CLIENTE");
            //DropIndex("dbo.CLIENTE_VEICULO", new[] { "Cliente_Id" });
            //DropIndex("dbo.CONVENIO", new[] { "TabelaPreco_Id" });
            //DropIndex("dbo.CONVENIO", new[] { "Cliente_Id" });
            //DropIndex("dbo.ORCAMENTO", new[] { "Cliente_Id" });
            AlterColumn("dbo.CLIENTE_VEICULO", "ClienteId", c => c.Long(nullable: false));
            AlterColumn("dbo.CONVENIO", "IdCliente", c => c.Long());
            AlterColumn("dbo.CONVENIO", "IdTabelaPreco", c => c.Long(nullable: false));
            AlterColumn("dbo.ORCAMENTO", "IdCliente", c => c.Long(nullable: false));
            //DropColumn("dbo.CLIENTE_VEICULO", "Cliente_Id");
            //DropColumn("dbo.CONVENIO", "TabelaPreco_Id");
            //DropColumn("dbo.CONVENIO", "Cliente_Id");
            DropColumn("dbo.CLIENTE", "IdCliente");
            //DropColumn("dbo.ORCAMENTO", "Cliente_Id");
            CreateIndex("dbo.CLIENTE_VEICULO", "ClienteId");
            CreateIndex("dbo.CONVENIO", "IdCliente");
            CreateIndex("dbo.CONVENIO", "IdTabelaPreco");
            CreateIndex("dbo.ORCAMENTO", "IdCliente");
            AddForeignKey("dbo.CLIENTE_VEICULO", "ClienteId", "dbo.CLIENTE", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CONVENIO", "IdTabelaPreco", "dbo.TABELA_PRECO", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CONVENIO", "IdCliente", "dbo.CLIENTE", "Id");
            AddForeignKey("dbo.ORCAMENTO", "IdCliente", "dbo.CLIENTE", "Id", cascadeDelete: true);
        }
    }
}
