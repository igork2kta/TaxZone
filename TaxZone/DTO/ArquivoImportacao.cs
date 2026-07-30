using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxZone.DTO
{
    public class ArquivoImportacao
    {
        public int GrupoArquivo { get; set; }
        public int NumeroArquivo { get; set; }
       // public string DescricaoArquivo { get; set; } = string.Empty;
        public string NomeTabelaWork { get; set; } = string.Empty;
       // public int QtdRegistros { get; set; }
       // public string IndAtoCotepe { get; set; } = string.Empty;
        public string IndEstabGrp { get; set; } = string.Empty;
       // public string IndMultiLoad { get; set; } = string.Empty;
    }
}
