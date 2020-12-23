namespace DV.FrenteLoja.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustes_DB30 : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "dbo.CONVENIO", newName: "Convenios");
            //RenameTable(name: "dbo.CONVENIO_CLIENTE", newName: "ConvenioClientes");
            //RenameTable(name: "dbo.CONVENIO_CONDICAO_PAGAMENTO", newName: "ConvenioCondicaoPagamentoes");
            //RenameTable(name: "dbo.CONVENIO_PRODUTO", newName: "ConvenioProdutoes");
            //DropIndex("dbo.Convenios", new[] { "CampoCodigo" });
            //DropIndex("dbo.ConvenioCondicaoPagamentoes", new[] { "CampoCodigo" });
            //DropIndex("dbo.ConvenioProdutoes", new[] { "CampoCodigo" });
            //AddColumn("dbo.ConvenioProdutoes", "Descricao", c => c.String());
            //AlterColumn("dbo.Convenios", "CampoCodigo", c => c.String());
            //AlterColumn("dbo.ConvenioCondicaoPagamentoes", "CampoCodigo", c => c.String());
            //AlterColumn("dbo.ConvenioProdutoes", "CampoCodigo", c => c.String());
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.ConvenioProdutoes", "CampoCodigo", c => c.String(maxLength: 12));
            //AlterColumn("dbo.ConvenioCondicaoPagamentoes", "CampoCodigo", c => c.String(maxLength: 12));
            //AlterColumn("dbo.Convenios", "CampoCodigo", c => c.String(maxLength: 12));
            //DropColumn("dbo.ConvenioProdutoes", "Descricao");
            //CreateIndex("dbo.ConvenioProdutoes", "CampoCodigo", unique: true);
            //CreateIndex("dbo.ConvenioCondicaoPagamentoes", "CampoCodigo", unique: true);
            //CreateIndex("dbo.Convenios", "CampoCodigo", unique: true);
            //RenameTable(name: "dbo.ConvenioProdutoes", newName: "CONVENIO_PRODUTO");
            //RenameTable(name: "dbo.ConvenioCondicaoPagamentoes", newName: "CONVENIO_CONDICAO_PAGAMENTO");
            //RenameTable(name: "dbo.ConvenioClientes", newName: "CONVENIO_CLIENTE");
            //RenameTable(name: "dbo.Convenios", newName: "CONVENIO");
        }
    }
}
