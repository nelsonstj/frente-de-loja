namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB32 : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.GRUPO_PRODUTO", "IdGrupoSubGrupo", "dbo.GRUPO_SUB_GRUPO");
            //DropIndex("dbo.GRUPO_PRODUTO", new[] { "IdGrupoSubGrupo" });
            //AlterColumn("dbo.GRUPO_PRODUTO", "IdGrupoSubGrupo", c => c.String());

            DropForeignKey("dbo.GRUPO_PRODUTO", "GrupoSubGrupo_Id", "dbo.GRUPO_SUB_GRUPO");
            DropIndex("dbo.GRUPO_PRODUTO", new[] { "GrupoSubGrupo_Id" });
            DropColumn("dbo.GRUPO_PRODUTO", "GrupoSubGrupo_Id");

            /*AddColumn("dbo.GRUPO_PRODUTO", "GrupoSubGrupo_Id", c => c.Long());
            CreateIndex("dbo.GRUPO_PRODUTO", "GrupoSubGrupo_Id");
            AddForeignKey("dbo.GRUPO_PRODUTO", "GrupoSubGrupo_Id", "dbo.GRUPO_SUB_GRUPO", "Id");*/
            /*
            DropForeignKey("dbo.GRUPO_SUB_GRUPO", "IdSubGrupo", "dbo.SUB_GRUPO");
            DropIndex("dbo.GRUPO_SUB_GRUPO", new[] { "IdSubGrupo" });
            //AddColumn("dbo.GRUPO_SUB_GRUPO", "SubGrupo_Id", c => c.Long());
            //CreateIndex("dbo.GRUPO_SUB_GRUPO", "SubGrupo_Id");
            //AddForeignKey("dbo.GRUPO_SUB_GRUPO", "SubGrupo_Id", "dbo.SUB_GRUPO", "Id");*/
        }

        public override void Down()
        {
            //AlterColumn("dbo.GRUPO_PRODUTO", "IdGrupoSubGrupo", c => c.Long());
            //CreateIndex("dbo.GRUPO_PRODUTO", "IdGrupoSubGrupo");
            //AddForeignKey("dbo.GRUPO_PRODUTO", "IdGrupoSubGrupo", "dbo.GRUPO_SUB_GRUPO", "Id");

            AddColumn("dbo.GRUPO_PRODUTO", "GrupoSubGrupo_Id", c => c.Long());
            CreateIndex("dbo.GRUPO_PRODUTO", "GrupoSubGrupo_Id");
            AddForeignKey("dbo.GRUPO_PRODUTO", "GrupoSubGrupo_Id", "dbo.GRUPO_SUB_GRUPO", "Id");

            /*DropForeignKey("dbo.GRUPO_PRODUTO", "GrupoSubGrupo_Id", "dbo.GRUPO_SUB_GRUPO");
            DropIndex("dbo.GRUPO_PRODUTO", new[] { "GrupoSubGrupo_Id" });
            DropColumn("dbo.GRUPO_PRODUTO", "GrupoSubGrupo_Id");*/
            /*
            //DropForeignKey("dbo.GRUPO_SUB_GRUPO", "SubGrupo_Id", "dbo.SUB_GRUPO");
            //DropIndex("dbo.GRUPO_SUB_GRUPO", new[] { "SubGrupo_Id" });
            //DropColumn("dbo.GRUPO_SUB_GRUPO", "SubGrupo_Id");
            CreateIndex("dbo.GRUPO_SUB_GRUPO", "IdSubGrupo");
            AddForeignKey("dbo.GRUPO_SUB_GRUPO", "IdSubGrupo", "dbo.SUB_GRUPO", "Id", cascadeDelete: true);*/
        }
    }
}
