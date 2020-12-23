namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB19 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PRODUTO", "IdGrupoProduto", "dbo.GRUPO_PRODUTO");
            DropForeignKey("dbo.GRUPO_PRODUTO", "IdGrupoSubGrupo", "dbo.GRUPO_SUB_GRUPO");
            DropForeignKey("dbo.GRUPO_SUB_GRUPO", "IdSubGrupo", "dbo.SUB_GRUPO");
            DropIndex("dbo.PRODUTO", new[] { "IdGrupoProduto" });
            DropIndex("dbo.GRUPO_PRODUTO", new[] { "IdGrupoSubGrupo" });
            DropIndex("dbo.GRUPO_SUB_GRUPO", new[] { "IdSubGrupo" });
            //******AddColumn("dbo.PRODUTO", "IdProduto", c => c.String());
            //AddColumn("dbo.PRODUTO", "GrupoProduto_Id", c => c.Long());
            //AddColumn("dbo.GRUPO_PRODUTO", "GrupoSubGrupo_Id", c => c.Long());
            //AddColumn("dbo.GRUPO_SUB_GRUPO", "SubGrupo_Id", c => c.Long());
            AlterColumn("dbo.PRODUTO", "IdGrupoProduto", c => c.String());
            AlterColumn("dbo.GRUPO_PRODUTO", "IdGrupoSubGrupo", c => c.String());
            //CreateIndex("dbo.PRODUTO", "GrupoProduto_Id");
            //CreateIndex("dbo.GRUPO_PRODUTO", "GrupoSubGrupo_Id");
            //CreateIndex("dbo.GRUPO_SUB_GRUPO", "SubGrupo_Id");
            //AddForeignKey("dbo.PRODUTO", "GrupoProduto_Id", "dbo.GRUPO_PRODUTO", "Id");
            //AddForeignKey("dbo.GRUPO_PRODUTO", "GrupoSubGrupo_Id", "dbo.GRUPO_SUB_GRUPO", "Id");
            //AddForeignKey("dbo.GRUPO_SUB_GRUPO", "SubGrupo_Id", "dbo.SUB_GRUPO", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.GRUPO_SUB_GRUPO", "SubGrupo_Id", "dbo.SUB_GRUPO");
            //DropForeignKey("dbo.GRUPO_PRODUTO", "GrupoSubGrupo_Id", "dbo.GRUPO_SUB_GRUPO");
            //DropForeignKey("dbo.PRODUTO", "GrupoProduto_Id", "dbo.GRUPO_PRODUTO");
            //DropIndex("dbo.GRUPO_SUB_GRUPO", new[] { "SubGrupo_Id" });
            //DropIndex("dbo.GRUPO_PRODUTO", new[] { "GrupoSubGrupo_Id" });
            //DropIndex("dbo.PRODUTO", new[] { "GrupoProduto_Id" });
            AlterColumn("dbo.GRUPO_PRODUTO", "IdGrupoSubGrupo", c => c.Long());
            AlterColumn("dbo.PRODUTO", "IdGrupoProduto", c => c.Long(nullable: false));
            //DropColumn("dbo.GRUPO_SUB_GRUPO", "SubGrupo_Id");
            //DropColumn("dbo.GRUPO_PRODUTO", "GrupoSubGrupo_Id");
            //DropColumn("dbo.PRODUTO", "GrupoProduto_Id");
            //*****DropColumn("dbo.PRODUTO", "IdProduto");
            CreateIndex("dbo.GRUPO_SUB_GRUPO", "IdSubGrupo");
            CreateIndex("dbo.GRUPO_PRODUTO", "IdGrupoSubGrupo");
            CreateIndex("dbo.PRODUTO", "IdGrupoProduto");
            AddForeignKey("dbo.GRUPO_SUB_GRUPO", "IdSubGrupo", "dbo.SUB_GRUPO", "Id", cascadeDelete: true);
            AddForeignKey("dbo.GRUPO_PRODUTO", "IdGrupoSubGrupo", "dbo.GRUPO_SUB_GRUPO", "Id");
            AddForeignKey("dbo.PRODUTO", "IdGrupoProduto", "dbo.GRUPO_PRODUTO", "Id", cascadeDelete: true);
        }
    }
}
