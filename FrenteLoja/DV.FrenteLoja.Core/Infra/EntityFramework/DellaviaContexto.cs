using DV.FrenteLoja.Core.Dominios.Entidades;
using System.Data.Entity;
using System.Reflection;

namespace DV.FrenteLoja.Core.Infra.EntityFramework
{
    public class DellaviaContexto : DbContext
    {
        public DellaviaContexto() : base("DellaviaContexto")
        {
            //DellaViaDBSeed.ExecutarSeed(this);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<vwVeiculos>().ToTable("VW_VEICULOS")
                .Ignore(a => a.Id)
                .Ignore(a => a.DataAtualizacao)
                .Ignore(a => a.UsuarioAtualizacao)
                .Ignore(a => a.Descricao)
                .Ignore(a => a.RegistroInativo);
            modelBuilder.Entity<VwVeiculoProdutos>().ToTable("VW_VEICULO_PRODUTOS")
                .Ignore(a => a.Id)
                .Ignore(a => a.DataAtualizacao)
                .Ignore(a => a.UsuarioAtualizacao)
                .Ignore(a => a.Descricao)
                .Ignore(a => a.RegistroInativo);
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<DV.FrenteLoja.Core.Dominios.Entidades.GrupoProduto> GrupoProduto { get; set; }
    }
}
