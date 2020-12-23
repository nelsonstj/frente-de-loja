namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdicionadoTabelaVeiculo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VEICULO",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IdFraga = c.String(nullable: false),
                        IdMarcaModeloVersao = c.Long(),
                        AnoInicial = c.DateTime(),
                        AnoFinal = c.DateTime(),
                        IdVersaoMotor = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MARCA_MODELO_VERSAO", t => t.IdMarcaModeloVersao)
                .ForeignKey("dbo.VERSAO_MOTOR", t => t.IdVersaoMotor)
                .Index(t => t.IdMarcaModeloVersao)
                .Index(t => t.IdVersaoMotor);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VEICULO", "IdVersaoMotor", "dbo.VERSAO_MOTOR");
            DropForeignKey("dbo.VEICULO", "IdMarcaModeloVersao", "dbo.MARCA_MODELO_VERSAO");
            DropIndex("dbo.VEICULO", new[] { "IdVersaoMotor" });
            DropIndex("dbo.VEICULO", new[] { "IdMarcaModeloVersao" });
            DropTable("dbo.VEICULO");
        }
    }
}
