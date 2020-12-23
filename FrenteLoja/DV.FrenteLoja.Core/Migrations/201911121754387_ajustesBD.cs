namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustesBD : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OPERADOR", "VendedorId", "dbo.VENDEDOR");
            DropForeignKey("dbo.ORCAMENTO", "IdOperador", "dbo.OPERADOR");
            DropForeignKey("dbo.ORCAMENTO", "IdVendedor", "dbo.VENDEDOR");
            DropIndex("dbo.ORCAMENTO", new[] { "IdVendedor" });
            DropIndex("dbo.ORCAMENTO", new[] { "IdOperador" });
            AddColumn("dbo.ORCAMENTO", "Vendedor_Id", c => c.Long());
            AddColumn("dbo.ORCAMENTO", "Operador_Id", c => c.Long());
            AddColumn("dbo.VENDEDOR", "IdConsultor", c => c.String());
            AddColumn("dbo.VENDEDOR", "IdLoja", c => c.String());
            AddColumn("dbo.VENDEDOR", "IdRegional", c => c.String());
            AddColumn("dbo.VENDEDOR", "IdUser", c => c.String());
            AlterColumn("dbo.ORCAMENTO", "IdVendedor", c => c.String());
            AlterColumn("dbo.ORCAMENTO", "IdOperador", c => c.String());
            CreateIndex("dbo.ORCAMENTO", "Vendedor_Id");
            CreateIndex("dbo.ORCAMENTO", "Operador_Id");
            AddForeignKey("dbo.ORCAMENTO", "Vendedor_Id", "dbo.VENDEDOR", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ORCAMENTO", "Vendedor_Id", "dbo.VENDEDOR");
            DropIndex("dbo.ORCAMENTO", new[] { "Operador_Id" });
            DropIndex("dbo.ORCAMENTO", new[] { "Vendedor_Id" });
            AlterColumn("dbo.ORCAMENTO", "IdOperador", c => c.Long(nullable: false));
            AlterColumn("dbo.ORCAMENTO", "IdVendedor", c => c.Long(nullable: false));
            DropColumn("dbo.VENDEDOR", "IdUser");
            DropColumn("dbo.VENDEDOR", "IdRegional");
            DropColumn("dbo.VENDEDOR", "IdLoja");
            DropColumn("dbo.VENDEDOR", "IdConsultor");
            DropColumn("dbo.ORCAMENTO", "Operador_Id");
            DropColumn("dbo.ORCAMENTO", "Vendedor_Id");
            CreateIndex("dbo.ORCAMENTO", "IdOperador");
            CreateIndex("dbo.ORCAMENTO", "IdVendedor");
            AddForeignKey("dbo.ORCAMENTO", "IdVendedor", "dbo.VENDEDOR", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ORCAMENTO", "IdOperador", "dbo.OPERADOR", "Id", cascadeDelete: true);
            AddForeignKey("dbo.OPERADOR", "VendedorId", "dbo.VENDEDOR", "Id");
        }
    }
}
