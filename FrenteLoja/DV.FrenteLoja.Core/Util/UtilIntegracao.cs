using DV.FrenteLoja.Core.Contratos.Enums;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DV.FrenteLoja.Core.Contratos.Interfaces;

namespace DV.FrenteLoja.Core.Util
{
    public static class UtilIntegracao
    {
        public static readonly IList<String> TipoProdutoPneus = new ReadOnlyCollection<string>
            (new List<String> { "0001", "0002", "0003", "0004", "0008", "0009", "0010", "0011",
                "0012", "0013", "0014", "0015", "0016", "0017", "0018", "0019", "0020", "0021",
                "0022", "0025", "0050", "0079", "0100", "0101", "0130", "0845", "1000" });

        public static readonly IList<String> TipoProdutoFreio = new ReadOnlyCollection<string>
            (new List<String> { "0501", "0502", "0504", "0508", "0512", "0550", "0551", "0557",
                "0607", "0608", "0609", "0610", "0624", "0764", "0865", "1490", "1492", "1501",
                "1502", "1503", "1505", "1508", "1512", "1513", "1515", "1535", "1810" });

        public static readonly IList<String> TipoProdutoLubrificantes = new ReadOnlyCollection<string>
            (new List<String> { "0617", "0620", "0621", "0623", "0635", "0636", "0637", "0639",
                "0642", "0644", "0800", "1800", "1804", "1808", "1815", "1816", "1817", "1818" });

        public static readonly IList<String> TipoProdutoServicos = new ReadOnlyCollection<string>
            (new List<String> { "0071", "0078", "0901", "0902", "0903", "0904", "0905", "0910",
                "0911", "0913", "0914", "0915", "0917", "0918", "1532" });

        public static readonly IList<String> TipoProdutoAcessorios = new ReadOnlyCollection<string>
            (new List<String> { "0026", "0027", "0028", "0029", "0030", "0031", "0032", "0033",
                "0034", "0035", "0036", "0037", "0038", "0039", "0040", "0041", "0042", "0043",
                "0044", "0045", "0046", "0047", "0048", "0049", "0069", "0070", "0077", "0085",
                "0086", "0099", "0515", "0570", "0601", "0602", "0603", "0604", "0605", "0606",
                "0611", "0612", "0613", "0615", "0622", "0627", "0628", "0630", "0632", "0645",
                "0647", "0650", "0651", "0652", "0653", "0654", "0656", "0660", "0661", "0664",
                "0665", "0666", "0668", "0680", "0682", "0690", "0700", "0701", "0702", "0703",
                "0720", "0731", "0805", "0815", "0846" });

        public static readonly IList<String> TipoProdutoSuspensao = new ReadOnlyCollection<string>
            (new List<String> { "0395", "0396", "0398", "0399", "0401", "0402", "0403", "0405",
                "0409", "0411", "0415", "0424", "0425", "0432", "0433", "0434", "0437", "0439",
                "0440", "0449", "0450", "0451", "0452", "0456", "0480", "0481", "0503", "0520",
                "0530", "0531", "0785", "0790", "0830", "0832", "0840", "0841", "0842", "0843",
                "0844", "0850", "0852", "0855", "0860", "0870", "0875", "0893", "1400", "1410",
                "1412", "1415", "1420", "1422", "1423", "1425", "1440", "1445", "1450", "1520",
                "1522", "1525", "1530", "1550", "1813", "1825", "1828", "1830", "1832", "1835",
                "1838", "1840", "1870", "1871", "1890" });
    }
}
