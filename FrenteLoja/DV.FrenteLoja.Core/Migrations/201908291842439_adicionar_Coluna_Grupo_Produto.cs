namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adicionar_Coluna_Grupo_Produto : DbMigration
    {
        public override void Up() {

			AddColumn("dbo.GRUPO_PRODUTO", "IdGrupoSubGrupo", c => c.Long(nullable: true));
			CreateIndex("dbo.GRUPO_PRODUTO", "IdGrupoSubGrupo");
            AddForeignKey("dbo.GRUPO_PRODUTO", "IdGrupoSubGrupo", "dbo.GRUPO_SUB_GRUPO", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GRUPO_PRODUTO", "IdGrupoSubGrupo", "dbo.GRUPO_SUB_GRUPO");
            DropIndex("dbo.GRUPO_PRODUTO", new[] { "IdGrupoSubGrupo" });
            DropColumn("dbo.GRUPO_PRODUTO", "IdGrupoSubGrupo");
        }
    }
}
