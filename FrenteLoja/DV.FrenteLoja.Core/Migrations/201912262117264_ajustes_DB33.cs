namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB33 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PRODUTO_COMPLEMENTO", "IdProduto", "dbo.PRODUTO");
            DropIndex("dbo.PRODUTO_COMPLEMENTO", new[] { "IdProduto" });
            //AddColumn("dbo.PRODUTO_COMPLEMENTO", "Produto_Id", c => c.Long());
            AlterColumn("dbo.PRODUTO_COMPLEMENTO", "IdProduto", c => c.String());
            //CreateIndex("dbo.PRODUTO_COMPLEMENTO", "Produto_Id");
            //AddForeignKey("dbo.PRODUTO_COMPLEMENTO", "Produto_Id", "dbo.PRODUTO", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.PRODUTO_COMPLEMENTO", "Produto_Id", "dbo.PRODUTO");
            //DropIndex("dbo.PRODUTO_COMPLEMENTO", new[] { "Produto_Id" });
            AlterColumn("dbo.PRODUTO_COMPLEMENTO", "IdProduto", c => c.Long(nullable: false));
            //DropColumn("dbo.PRODUTO_COMPLEMENTO", "Produto_Id");
            CreateIndex("dbo.PRODUTO_COMPLEMENTO", "IdProduto");
            AddForeignKey("dbo.PRODUTO_COMPLEMENTO", "IdProduto", "dbo.PRODUTO", "Id", cascadeDelete: true);
        }
    }
}
