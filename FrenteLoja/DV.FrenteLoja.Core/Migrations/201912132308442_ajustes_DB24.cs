namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB24 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GRUPO_PRODUTO", "IdGrupoProduto", c => c.String());
            AddColumn("dbo.GRUPO_SUB_GRUPO", "IdGrupoSubGrupo", c => c.String());
            AlterColumn("dbo.CONVENIO_PRODUTO", "IdProduto", c => c.String());
            AlterColumn("dbo.CONVENIO_PRODUTO", "IdGrupoProduto", c => c.String());
            AlterColumn("dbo.GRUPO_SUB_GRUPO", "IdSubGrupo", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GRUPO_SUB_GRUPO", "IdSubGrupo", c => c.Long(nullable: false));
            AlterColumn("dbo.CONVENIO_PRODUTO", "IdGrupoProduto", c => c.Long());
            AlterColumn("dbo.CONVENIO_PRODUTO", "IdProduto", c => c.Long());
            DropColumn("dbo.GRUPO_SUB_GRUPO", "IdGrupoSubGrupo");
            DropColumn("dbo.GRUPO_PRODUTO", "IdGrupoProduto");
        }
    }
}
