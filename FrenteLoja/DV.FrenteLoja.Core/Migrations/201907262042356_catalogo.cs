namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class catalogo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VEICULO_MEDIDAS_PNEUS",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        idVeiculo = c.Long(nullable: false),
                        Aro = c.Decimal(nullable: false),
                        Largura = c.Decimal(nullable: false),
                        Perfil = c.Decimal(nullable: false),
                        Carga = c.Decimal(nullable: false),
                        Indice = c.String(nullable: false),
                        Posicao = c.Int(nullable: false),
                        Descricao = c.String(),
                        RegistroInativo = c.Boolean(nullable: false),
                        CampoCodigo = c.String(),
                        DataAtualizacao = c.DateTime(nullable: false),
                        UsuarioAtualizacao = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VEICULO", t => t.idVeiculo, cascadeDelete: true)
                .Index(t => t.idVeiculo);
            
            AddColumn("dbo.CATALOGO", "VersaoMotor", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VEICULO_MEDIDAS_PNEUS", "idVeiculo", "dbo.VEICULO");
            DropIndex("dbo.VEICULO_MEDIDAS_PNEUS", new[] { "idVeiculo" });
            DropColumn("dbo.CATALOGO", "VersaoMotor");
            DropTable("dbo.VEICULO_MEDIDAS_PNEUS");
        }
    }
}
