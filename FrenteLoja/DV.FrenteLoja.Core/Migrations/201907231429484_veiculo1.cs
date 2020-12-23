namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class veiculo1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CLIENTE_VEICULO", "MarcaModeloVersaoId", "dbo.MARCA_MODELO_VERSAO");
            DropIndex("dbo.CLIENTE_VEICULO", new[] { "MarcaModeloVersaoId" });
            AddColumn("dbo.ORCAMENTO", "VeiculoId", c => c.Long(nullable: false));
            AddColumn("dbo.CLIENTE_VEICULO", "VeiculoId", c => c.Long(nullable: false));
            CreateIndex("dbo.ORCAMENTO", "VeiculoId");
            CreateIndex("dbo.CLIENTE_VEICULO", "VeiculoId");
            AddForeignKey("dbo.CLIENTE_VEICULO", "VeiculoId", "dbo.VEICULO", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ORCAMENTO", "VeiculoId", "dbo.VEICULO", "Id", cascadeDelete: true);
            DropColumn("dbo.CLIENTE_VEICULO", "MarcaModeloVersaoId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CLIENTE_VEICULO", "MarcaModeloVersaoId", c => c.Long(nullable: false));
            DropForeignKey("dbo.ORCAMENTO", "VeiculoId", "dbo.VEICULO");
            DropForeignKey("dbo.CLIENTE_VEICULO", "VeiculoId", "dbo.VEICULO");
            DropIndex("dbo.CLIENTE_VEICULO", new[] { "VeiculoId" });
            DropIndex("dbo.ORCAMENTO", new[] { "VeiculoId" });
            DropColumn("dbo.CLIENTE_VEICULO", "VeiculoId");
            DropColumn("dbo.ORCAMENTO", "VeiculoId");
            CreateIndex("dbo.CLIENTE_VEICULO", "MarcaModeloVersaoId");
            AddForeignKey("dbo.CLIENTE_VEICULO", "MarcaModeloVersaoId", "dbo.MARCA_MODELO_VERSAO", "Id", cascadeDelete: true);
        }
    }
}
