namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class criacao_tabela_grupo_e_subgrupo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GRUPO_SUB_GRUPO",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IdSubGrupo = c.Long(nullable: false),
                        Grupo = c.Int(nullable: false),
                        Descricao = c.String(),
                        DataAtualizacao = c.DateTime(nullable: false),
                        UsuarioAtualizacao = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SUB_GRUPO", t => t.IdSubGrupo, cascadeDelete: true)
                .Index(t => t.IdSubGrupo);
            
            CreateTable(
                "dbo.SUB_GRUPO",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Descricao = c.String(),
                        DataAtualizacao = c.DateTime(nullable: false),
                        UsuarioAtualizacao = c.String(),
                    })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GRUPO_SUB_GRUPO", "IdSubGrupo", "dbo.SUB_GRUPO");
            DropIndex("dbo.GRUPO_SUB_GRUPO", new[] { "IdSubGrupo" });
            DropTable("dbo.SUB_GRUPO");
            DropTable("dbo.GRUPO_SUB_GRUPO");
        }
    }
}
