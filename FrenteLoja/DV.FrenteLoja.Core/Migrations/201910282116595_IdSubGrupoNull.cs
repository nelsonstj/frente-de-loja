namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdSubGrupoNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GRUPO_SUB_GRUPO", "IdSubGrupo", c => c.Long(nullable: true));
        }

        public override void Down()
        {
            AlterColumn("dbo.GRUPO_SUB_GRUPO", "IdSubGrupo", c => c.Long(nullable: false));
        }
    }
}
