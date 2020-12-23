namespace DV.FrenteLoja.Core.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AdicionadoTabelaVersaoMotor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VERSAO_MOTOR",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Descricao = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.VERSAO_MOTOR");
        }
    }
}
