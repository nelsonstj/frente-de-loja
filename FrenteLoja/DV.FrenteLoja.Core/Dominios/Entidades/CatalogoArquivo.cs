using DV.FrenteLoja.Core.Contratos.Abstratos;

namespace DV.FrenteLoja.Core.Dominios.Entidades
{
    public class CatalogoArquivo : Entidade
    {
        public string Nome { get; set; }
        public byte[] Arquivo { get; set; }
    }
}
