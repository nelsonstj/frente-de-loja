namespace DV.FrenteLoja.Core.Contratos.DataObjects
{
    public class CargaCatalogoDto
    {
        public string Nome { get; set; }
        public long Tamanho { get; set; }
        public byte[] Arquivo { get; set; }
        public string LogImportacao { get; set; }
    }
}
