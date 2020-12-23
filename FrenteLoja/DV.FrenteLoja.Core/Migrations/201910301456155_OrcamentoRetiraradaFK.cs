namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrcamentoRetiraradaFK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ORCAMENTO", "VeiculoId", "dbo.VEICULO");
            DropForeignKey("dbo.CLIENTE_VEICULO", "VeiculoId", "dbo.VEICULO");
            DropForeignKey("dbo.ORCAMENTO", "IdMarcaModeloVersao", "dbo.MARCA_MODELO_VERSAO");
            DropIndex("dbo.ORCAMENTO", new[] { "IdMarcaModeloVersao" });
            DropIndex("dbo.ORCAMENTO", new[] { "VeiculoId" });
            DropIndex("dbo.CLIENTE_VEICULO", new[] { "VeiculoId" });
            RenameColumn(table: "dbo.CLIENTE_VEICULO", name: "VeiculoId", newName: "Veiculo_Id");
            RenameColumn(table: "dbo.ORCAMENTO", name: "IdMarcaModeloVersao", newName: "MarcaModeloVersao_Id");
            AddColumn("dbo.ORCAMENTO", "VeiculoIdFraga", c => c.String());
            AddColumn("dbo.CLIENTE_VEICULO", "VeiculoIdFraga", c => c.String());
            AlterColumn("dbo.ORCAMENTO", "MarcaModeloVersao_Id", c => c.Long());
            AlterColumn("dbo.CLIENTE_VEICULO", "Veiculo_Id", c => c.Long());
            CreateIndex("dbo.ORCAMENTO", "MarcaModeloVersao_Id");
            CreateIndex("dbo.CLIENTE_VEICULO", "Veiculo_Id");
            AddForeignKey("dbo.CLIENTE_VEICULO", "Veiculo_Id", "dbo.VEICULO", "Id");
            AddForeignKey("dbo.ORCAMENTO", "MarcaModeloVersao_Id", "dbo.MARCA_MODELO_VERSAO", "Id");
            DropColumn("dbo.ORCAMENTO", "VeiculoId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ORCAMENTO", "VeiculoId", c => c.Long(nullable: false));
            DropForeignKey("dbo.ORCAMENTO", "MarcaModeloVersao_Id", "dbo.MARCA_MODELO_VERSAO");
            DropForeignKey("dbo.CLIENTE_VEICULO", "Veiculo_Id", "dbo.VEICULO");
            DropIndex("dbo.CLIENTE_VEICULO", new[] { "Veiculo_Id" });
            DropIndex("dbo.ORCAMENTO", new[] { "MarcaModeloVersao_Id" });
            AlterColumn("dbo.CLIENTE_VEICULO", "Veiculo_Id", c => c.Long(nullable: false));
            AlterColumn("dbo.ORCAMENTO", "MarcaModeloVersao_Id", c => c.Long(nullable: false));
            DropColumn("dbo.CLIENTE_VEICULO", "VeiculoIdFraga");
            DropColumn("dbo.ORCAMENTO", "VeiculoIdFraga");
            RenameColumn(table: "dbo.ORCAMENTO", name: "MarcaModeloVersao_Id", newName: "IdMarcaModeloVersao");
            RenameColumn(table: "dbo.CLIENTE_VEICULO", name: "Veiculo_Id", newName: "VeiculoId");
            CreateIndex("dbo.CLIENTE_VEICULO", "VeiculoId");
            CreateIndex("dbo.ORCAMENTO", "VeiculoId");
            CreateIndex("dbo.ORCAMENTO", "IdMarcaModeloVersao");
            AddForeignKey("dbo.ORCAMENTO", "IdMarcaModeloVersao", "dbo.MARCA_MODELO_VERSAO", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CLIENTE_VEICULO", "VeiculoId", "dbo.VEICULO", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ORCAMENTO", "VeiculoId", "dbo.VEICULO", "Id", cascadeDelete: true);
        }
    }
}
