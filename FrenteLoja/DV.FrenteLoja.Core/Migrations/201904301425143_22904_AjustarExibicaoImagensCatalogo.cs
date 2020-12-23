namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class _22904_AjustarExibicaoImagensCatalogo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PRODUTO_COMPLEMENTO", "Perfil", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.PRODUTO_COMPLEMENTO", "Aro", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.PRODUTO_COMPLEMENTO", "Carga", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.PRODUTO_COMPLEMENTO", "Indice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }

        public override void Down()
        {
            DropColumn("dbo.PRODUTO_COMPLEMENTO", "Indice");
            DropColumn("dbo.PRODUTO_COMPLEMENTO", "Carga");
            DropColumn("dbo.PRODUTO_COMPLEMENTO", "Aro");
            DropColumn("dbo.PRODUTO_COMPLEMENTO", "Perfil");
        }
    }
}
